using System;

// Data structure from where you can efficently acces the biggest item.
public class Heap<T> where T : IHeapItem<T>
{

	T[] items;
	int currentItemCount;

	// Constructer creates underlying array.
	public Heap()
	{
		items = new T[32];
	}

	// Adds item to Heap.
	public void Add(T item)
	{
		item.HeapIndex = currentItemCount;
		if(currentItemCount >= items.Length)
        {
			Array.Resize(ref items, items.Length * 2);
        }
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

	// Removes first item while also returning that first item.
	public T RemoveFirst()
	{
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}

	// Updates position of item inside heap.
	public void UpdateItem(T item)
	{
		SortUp(item);
	}

	// Returns the amount of elements in the heap.
	public int Count
	{
		get
		{
			return currentItemCount;
		}
	}

	// Reruns if heap contains item or not.
	public bool Contains(T item)
	{
		if(item.HeapIndex >= items.Length)
		{
			return false;
        }
		return Equals(items[item.HeapIndex], item);
	}

	// Compares item to its two children and swaps with one of them if it is smaller
	// and repeat till it is larger than bouth its children or there are no children.
	void SortDown(T item)
	{
		while (true)
		{
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex;

			if (childIndexLeft < currentItemCount)
			{
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount)
				{
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
					{
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0)
				{
					Swap(item, items[swapIndex]);
				}
				else
				{
					return;
				}

			}
			else
			{
				return;
			}

        }
	}

	// Compares item to its parent and swaps if it is larger than the parent until
	// the parent is larger or there is no parent.
	void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2;

		while (true)
		{
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0)
			{
				Swap(item, parentItem);
			}
			else
			{
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	// Swaps two items.
	void Swap(T itemA, T itemB)
	{
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

// Heap item containt methods of IComparable and a heapindex
public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex
	{
		get;
		set;
	}
}
