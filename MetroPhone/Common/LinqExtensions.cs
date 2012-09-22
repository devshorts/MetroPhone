using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroPhone.Common
{
	public static class LinqExtensions
	{
		

		public static IEnumerable<TAccumulated> CollectList<TSource, TAccumulated>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TAccumulated>> getListFucntion)
		{
			return source.Aggregate(
				new List<TAccumulated>(),
				(accumulator, tSource) => { accumulator.AddRange(getListFucntion(tSource)); return accumulator; });
		}

		public static IEnumerable<T> FlattenList<T>(this IEnumerable<IEnumerable<T>> source)
		{
			return source.Aggregate(
				new List<T>(),
				(accumulator, tSource) => { accumulator.AddRange(tSource); return accumulator; });
		}

		public static TAccumulator Fold<TSource, TAccumulator>(this IEnumerable<TSource> source, TAccumulator seed, Func<TAccumulator, TSource, TAccumulator> foldFunction)
		{
			return source.Aggregate(seed, foldFunction);
		}

		public static string FoldToCommaDelimitedList<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatItemFunc)
		{
			return source.FoldToDelimitedList(formatItemFunc, ',');
		}

		public static string FoldToDelimitedList<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatItemFunc, char delimiter)
		{
			return source
				.Aggregate(new StringBuilder(), (acc, item) => acc.AppendFormat("{0}{1}", formatItemFunc(item), delimiter))
				.ToString()
				.Trim(new[] { ' ', delimiter });
		}

		public static string FoldToDelimitedList<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatItemFunc, string delimiter)
		{
			var s = source
				.Aggregate(new StringBuilder(), (acc, item) => acc.AppendFormat("{0}{1}", formatItemFunc(item), delimiter))
				.ToString();
			return s.EndsWith(delimiter)
				? s.Substring(0, s.Length - delimiter.Length)
				: s;
		}

		public static string FoldToLines<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatItemFunc)
		{
			return source.FoldToDelimitedList(formatItemFunc, "\r\n");
		}

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
        	if (enumerable == null)
        	{
        		throw new ArgumentNullException("enumerable");
        	}

			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			foreach(T t in enumerable)
			{
				action(t);
			}
        }
		
		public static bool IsSubsetOf<T>(this IEnumerable<T> subSet, IEnumerable<T> superSet)
		{

			foreach (var element in subSet)
			{
				if (!superSet.Contains(element)) return false;
			}

			return true;
		}

		public static bool NotEmpty(this string text)
		{
			return !string.IsNullOrEmpty(text);
		}

		

		public static Dictionary<TFirst, TSecond> ZipToDictionary<TFirst, TSecond>(this IEnumerable<TFirst> s1, IEnumerable<TSecond> s2)
		{
			return s1.Combine(s2, (a, b) => new { Key = a, Value = b })
					 .Fold(new Dictionary<TFirst, TSecond>(),
						   (dict, keyValue) =>
						   {
							   dict.Add(keyValue.Key, keyValue.Value);
							   return dict;
						   });
		}
        
        

       public static Dictionary<TKey, TSource> ToUniqueDictionary<TSource, TKey>(this IEnumerable<TSource> source,
                                                                    Func<TSource, TKey> keySelector)
        {
            return source.DisticntWithFunction((a, b) => keySelector(a).Equals(keySelector(b))).ToDictionary(keySelector);
        }


		public static Dictionary<TKey, TValue> KeyValuePairsToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
		{
			return keyValuePairs
					 .Fold(new Dictionary<TKey, TValue>(),
						   (dict, pair) =>
						   {
							   dict.Add(pair.Key, pair.Value);
							   return dict;
						   });
		}

		public static IEnumerable<TCombined> Combine<TFirst, TSecond, TCombined>(this IEnumerable<TFirst> s1, IEnumerable<TSecond> s2, Func<TFirst, TSecond, TCombined> combineFunc)
		{
			var s1Count = s1.Count();
			var s2Count = s2.Count();
			if (s1Count != s2Count)
			{
				throw new ArgumentException(
					string.Format(
						"Combine requires that the two enumerations are of equal length. But the first enumeration has {0} elements, while the other has {1} elements.",
						s1Count, s2Count));
			}

			using (var e1 = s1.GetEnumerator())
			using (var e2 = s2.GetEnumerator())
			{
				while (e1.MoveNext() && e2.MoveNext())
				{
					yield return combineFunc(e1.Current, e2.Current);
				}
			}

		}

		public static bool ContainsSameDistinctValues<T>(this IEnumerable<T> set1, IEnumerable<T> set2)
		{
			return set1.Except(set2).Count() == 0;
		}

		public static bool ContainsEqual<T>(this IEnumerable<T> set, T item, Func<T, T, bool> equalityFunc)
		{
			return set.Any(s => equalityFunc(s, item));
		}

		public static IEnumerable<T> DisticntWithFunction<T>(this IEnumerable<T> set, Func<T, T, bool> equalityFunc)
		{
			var list = new List<T>();

			foreach (var s in set)
			{
				if (!list.ContainsEqual(s, equalityFunc)) list.Add(s);
			}

			return list;
		}

		public static bool EqualSequence<T>(this IEnumerable<T> sequence1, IEnumerable<T> sequence2, Func<T, T, bool> equalityFunc)
		{
			if (sequence1.Count() != sequence2.Count())
			{
				return false;
			}

			return sequence1
				.Combine(sequence2, (a, b) => new { a, b })
				.All(x => equalityFunc(x.a, x.b));

		}


		public static bool StartsWith<T>(this IEnumerable<T> sequence1, IEnumerable<T> startWith)
		{

			return sequence1.Take(startWith.Count()).EqualSequence(startWith, (t0, t1) => t0.Equals(t1));
		}

		public static bool StartsWith<T>(this IEnumerable<T> sequence1, IEnumerable<T> startWith, Func<T, T, bool> equalityFunc)
		{
			if (sequence1.Count() < startWith.Count())
			{
				return false;
			}

			return sequence1.Take(startWith.Count()).EqualSequence(startWith, equalityFunc);
		}

		public static IEnumerable<T> ExceptEqual<T>(this IEnumerable<T> set0, IEnumerable<T> set1, Func<T, T, bool> equalFunc)
		{
			return (from s0 in set0
					from s1 in set1
					where !set1.ContainsEqual(s0, equalFunc)
					select s0).DisticntWithFunction(equalFunc);
		}

		

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> sequence)
		{
			if (sequence == null) return true;

		    return !sequence.Any();
		}

        /// <summary>
        /// Just in case someone uses IEnumerable&lt;T&gt;.IsNullOrEmpty() by mistake
        /// on a string since that's a bad thing to do.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return String.IsNullOrEmpty(text);
        }

		
		public static TAccumulator FoldLeaves<TSource, TAccumulator>(this IEnumerable<TSource> sequence, 
			Func<TSource, IEnumerable<TSource>> getNexLevel,
			TAccumulator accumulator, Func<TAccumulator, TSource, TAccumulator> foldFunction)
		{
			return sequence.Fold(accumulator, (acc, value) =>
			                           	{
			                           		var nextLevel = getNexLevel(value);
			                           		return nextLevel.IsNullOrEmpty()
			                           		       	? foldFunction(accumulator, value)
			                           		       	: FoldLeaves(nextLevel, getNexLevel,
			                           		       	             acc, foldFunction);
			                           	});


		}


		public static TAccumulator FoldBranchesAndLeaves<TSource, TAccumulator>(this IEnumerable<TSource> sequence,
			Func<TSource, IEnumerable<TSource>> getNexLevel,
			TAccumulator accumulator, Func<TAccumulator, TSource, TAccumulator> foldFunction)
		{
			return sequence.Fold(accumulator, (acc, value) =>
										{
											var nextLevel = getNexLevel(value);
											return nextLevel.IsNullOrEmpty()
													? foldFunction(accumulator, value)
													: FoldBranchesAndLeaves(nextLevel, getNexLevel,
																 foldFunction(acc, value), foldFunction);
										});


		}

		public static IEnumerable<TCollected> CollectLeaves<TSource, TCollected>(this IEnumerable<TSource> source,
			Func<TSource, IEnumerable<TSource>> getNexLevel,
			Func<TSource, TCollected> getValueFucntion)
		{
			return source.FoldLeaves(
				getNexLevel,
				new List<TCollected>(),
				(accumulator, tSource) => { accumulator.Add(getValueFucntion(tSource)); return accumulator; });
		}

		public static IEnumerable<TCollected> CollectBranchesAndLeaves<TSource, TCollected>(this IEnumerable<TSource> source,
			Func<TSource, IEnumerable<TSource>> getNexLevel,
			Func<TSource, TCollected> getValueFucntion)
		{
			return source.FoldBranchesAndLeaves(
				getNexLevel,
				new List<TCollected>(),
				(accumulator, tSource) => { accumulator.Add(getValueFucntion(tSource)); return accumulator; });
		}

		public static TElement MinElement<TElement, TComparable>(this IEnumerable<TElement> source, 
			Func<TElement, TComparable> selectorFunc) where TComparable : IComparable
		{
			if(source.IsNullOrEmpty())
			{
				throw new Exception(string.Format("MinElement extension method cannot be used on empty collection"));
			}

			var minElement = source.First();
			var minValue = selectorFunc(minElement);

			foreach(var element in source.Skip(1))
			{
				var elementValue = selectorFunc(element);
				if(minValue.CompareTo(elementValue) > 0 )
				{
					minValue = elementValue;
					minElement = element;
				}
			}

			return minElement;
		}

		public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> items)
		{
			return source.Except(items).Count() < source.Count();
		}

	
		public static IEnumerable<T> Not<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			return source.Where(t => !predicate(t));
		}

		public static IEnumerable<T> IntersectionOfAll<T>(this IEnumerable<IEnumerable<T>> sequences)
		{
			if( sequences.Count() == 0) return new T[0];
			return sequences.Skip(1).Aggregate(sequences.First(), (intersection, sequence) => intersection.Intersect(sequence));
		}

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource max = sourceIterator.Current;
                TKey maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

		public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(v => v);
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending order by using a specified comparer.  Equivalent to built-in source.OrderBy(t => t, comparer).
		/// </summary>
		/// <typeparam name="T">The type of the elements of source</typeparam>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="comparer">An System.Collections.Generic.IComparer&lt;T> to compare values.</param>
		/// <returns>An System.Linq.IOrderedEnumerable&lt;T> whose elements are sorted according to a key.</returns>
		public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, IComparer<T> comparer)
		{
			return source.OrderBy(t => t, comparer);
		}

		private class DynamicObjectComparer<T> : IEqualityComparer<T>
		{
			private readonly Func<T, T, bool> _equals;
			private readonly Func<T, int> _hashCode;

			public DynamicObjectComparer(Func<T, T, bool> equals, Func<T, int> hashCode)
			{
				_equals = equals;
				_hashCode = hashCode;
			}

			public bool Equals(T x, T y)
			{
				return _equals(x, y);
			}

			public int GetHashCode(T obj)
			{
				return _hashCode(obj);
			}
		}
	}
}