namespace ClassLibrary1
{
    public class DoublyLinkedList<T>
    {
        public LinkedListNode<T> Root { get; set; }
    }

    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Prev { get; set; }
        public LinkedListNode<T> Next { get; set; }

        public void InsertBefore(LinkedListNode<T> other)
        {
            
        }

        public void InsertAfter(LinkedListNode<T> other)
        {
            
        }

        public void SwapPosition(LinkedListNode<T> other)
        {
            
        }
    }
}