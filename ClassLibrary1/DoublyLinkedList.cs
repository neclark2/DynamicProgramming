namespace ClassLibrary1
{
    public class DoublyLinkedList<T>
    {
        public LinkedListNode<T> Root { get; private set; }
        public LinkedListNode<T> End { get; private set; }
        public int Length { get; private set; }

        public void Append(T item)
        {
            var node = new LinkedListNode<T>() {Value = item};

            if (Root == null)
            {
                Root = End = node;
                Length = 1;
            }
            else
            {
                End.Next = node;
                node.Prev = End;
                End = node;
                Length++;
            }
        }

        public void SwapPosition(LinkedListNode<T> first, LinkedListNode<T> second)
        {
            if (first == second)
            {
                return;
            }

            var firstNext = first.Next;
            var firstPrev = first.Prev;

            var secondNext = second.Next;
            var secondPrev = second.Prev;

            if (secondNext != null && secondNext != first)
            {
                secondNext.Prev = first;
            }

            if (secondPrev != null && secondPrev != first)
            {
                secondPrev.Next = first;
            }

            if (firstNext != null && firstNext != second)
            {
                firstNext.Prev = second;
            }

            if (firstPrev != null && firstPrev != second)
            {
                firstPrev.Next = second;
            }

            first.Next = (secondNext == first) ? second : secondNext;
            first.Prev = (secondPrev == first) ? second : secondPrev;
            second.Next = (firstNext == second) ? first : firstNext;
            second.Prev = (firstPrev == second) ? first : firstPrev;

            if (first == Root)
            {
                Root = second;
            }

            if (first == End)
            {
                End = second;
            }

            if (second == Root)
            {
                Root = first;
            }

            if (second == End)
            {
                End = first;
            }
        }


        public void PlaceBefore(LinkedListNode<T> pullNode, LinkedListNode<T> position)
        {
            if (pullNode == position)
            {
                return;
            }

            if (pullNode.Next != null)
            {
                pullNode.Next.Prev = pullNode.Prev;
            }

            if (pullNode.Prev != null)
            {
                pullNode.Prev.Next = pullNode.Next;
            }

            if (position.Prev != null)
            {
                position.Prev.Next = pullNode;
            }

            pullNode.Prev = position.Prev;
            pullNode.Next = position;
            position.Prev = pullNode;

            if (Root == position)
            {
                Root = pullNode;
            }
        }

        public void PlaceAfter(LinkedListNode<T> pullNode, LinkedListNode<T> position)
        {
            if (pullNode == position)
            {
                return;
            }
            
            if (position.Next != null)
            {
                position.Next.Prev = pullNode;
            }

            pullNode.Next = position.Next;
            pullNode.Prev = position;
            position.Next = pullNode;

        }

    }

    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Prev { get; set; }
        public LinkedListNode<T> Next { get; set; }

       

       
    }
}