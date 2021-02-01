# ARC-Utility

This is a collection of personal scripts to make serialization in Unity more easy. Feel free to use in your personal games if you would like.
The project has a few simple example scripts that show how to use this utility in your own games :)

# __Important Classes__

## RList
* This is a property attribute / property drawer that displays lists as a ReorderableList.
It can be used either as part of a custom editor script or you can attach the attribute next to your list to easily change the display.

* ReorderableList is an internal unity type that lets you change the order of elements and delete specific elements.
You have definitely seen it before since its used all over the place in Unity including for displaying UnityEvents.

Note: Due to how serialization works in Unity, you must used ListWrapper<T> instead of List<T> or Array[T] -- See Example code.

This is because we need to apply the custom PropertyAttribute to the list itself not to each individual as using List<T> would do.\
-- ListWrapper<T> is merely a wrapper around List<T>. It can be used exactly the same as List. Same methods. Same interfaces.

## MixedList<TBase, T1, T2>
* This has the same display as RList. However it can hold multiple different classes that inherit from a given base class (or interface).

* You can use this to serialize an list of abstract class instances, interface implementors, etc. 

* Use when you want your polymorphic lists serialized correctly

## UDictionary<TKey, TValue>
* This is a basic dictionary class that I'm using as a base to create serializable dictionaries. 

* It needs to be extended to be used with a given key type. This is because each key must be unique and the dictionary needs to be able
to generate new keys if a collision occurs while serializing.

* More work ironing this class out in the future is planned. Mainly to make it a bit easier to use. In it's current state it should
save some time creating runtime dictionaries without needing custom serialization code.

(I have already made a few classes like StringDictionary<T> and EnumDictionary<T> since these are the most common dictionaries I find myself using)
