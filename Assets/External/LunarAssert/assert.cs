#if DEBUG && UNITY_EDITOR
#define ASSERTS_ENABLED
#endif

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Text.RegularExpressions;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

using LunarAssertsInternal;

public delegate void AssertCallback(string message, string stackTrace);

public interface IAssertImp
{
    void OnAssert(string message, string stackTrace);
}

public static class assert
{
    /// <summary>
    /// Real number comparation precision
    /// </summary>
    public const float Epsilon = 0.00001f;

    public static IAssertImp assertImp;
    public static bool enabled;

    public static event AssertCallback onAssertCallback;

    #if ASSERTS_ENABLED
    static assert()
    {
        #if UNITY_EDITOR
        assertImp = new EditorAssertImp();
        enabled = true;
        #endif // UNITY_EDITOR
    }
    #endif

    #region True/False

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsTrue(bool condition)
    {
        if (enabled && !condition)
            AssertHelper("Assertion failed: 'true' expected");
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsTrue(bool condition, string message)
    {
        if (enabled && !condition)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsTrue(bool condition, string format, params object[] args)
    {
        if (enabled && !condition)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsTrue<A0>(bool condition, string format, A0 arg0)
    {
        if (enabled && !condition)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsTrue<A0, A1>(bool condition, string format, A0 arg0, A1 arg1)
    {
        if (enabled && !condition)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsTrue<A0, A1, A2>(bool condition, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && !condition)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsFalse(bool condition)
    {
        if (enabled && condition)
            AssertHelper("Assertion failed: 'false' expected");
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsFalse(bool condition, string message)
    {
        if (enabled && condition)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsFalse(bool condition, string format, params object[] args)
    {
        if (enabled && condition)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsFalse<A0>(bool condition, string format, A0 arg0)
    {
        if (enabled && condition)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsFalse<A0, A1>(bool condition, string format, A0 arg0, A1 arg1)
    {
        if (enabled && condition)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsFalse<A0, A1, A2>(bool condition, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && condition)
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Nullability

    public static T NotNull<T>(T obj) where T : class
    {
        #if ASSERTS_ENABLED
        if (enabled && obj == null)
        {
            AssertHelper("Assertion failed: expected 'null' but was '{0}'", obj);
        }
        #endif

        return obj;
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNull(object obj)
    {
        if (enabled && obj != null)
            AssertHelper("Assertion failed: expected 'null' but was '{0}'", obj);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNull(object obj, string message)
    {
        if (enabled && obj != null)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNull(object obj, string format, params object[] args)
    {
        if (enabled && obj != null)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNull<A0>(object obj, string format, A0 arg0)
    {
        if (enabled && obj != null)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNull<A0, A1>(object obj, string format, A0 arg0, A1 arg1)
    {
        if (enabled && obj != null)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNull<A0, A1, A2>(object obj, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && obj != null)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNull(object obj)
    {
        if (enabled && obj == null)
            AssertHelper("Assertion failed: object is 'null'");
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNull(object obj, string message)
    {
        if (enabled && obj == null)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNull(object obj, string format, params object[] args)
    {
        if (enabled && obj == null)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNullElement<T>(IList<T> list)
        where T : class
    {
        if (enabled)
        {
            int index = 0;
            foreach (T t in list)
            {
                assert.IsNotNull(t, "Element at {0} is null", index.ToString());
                ++index;
            }
        }
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNull<A0>(object obj, string format, A0 arg0)
    {
        if (enabled && obj == null)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNull<A0, A1>(object obj, string format, A0 arg0, A1 arg1)
    {
        if (enabled && obj == null)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotNull<A0, A1, A2>(object obj, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && obj == null)
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Equality

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(bool expected, bool actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(bool expected, bool actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(bool expected, bool actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(bool expected, bool actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(bool expected, bool actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(bool expected, bool actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(byte expected, byte actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(byte expected, byte actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(byte expected, byte actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(byte expected, byte actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(byte expected, byte actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(byte expected, byte actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(sbyte expected, sbyte actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(sbyte expected, sbyte actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(sbyte expected, sbyte actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(sbyte expected, sbyte actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(sbyte expected, sbyte actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(sbyte expected, sbyte actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(short expected, short actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(short expected, short actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(short expected, short actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(short expected, short actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(short expected, short actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(short expected, short actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(ushort expected, ushort actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(ushort expected, ushort actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(ushort expected, ushort actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(ushort expected, ushort actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(ushort expected, ushort actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(ushort expected, ushort actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(char expected, char actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(char expected, char actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(char expected, char actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(char expected, char actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(char expected, char actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(char expected, char actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(int expected, int actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(int expected, int actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(int expected, int actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(int expected, int actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(int expected, int actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(int expected, int actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(uint expected, uint actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(uint expected, uint actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(uint expected, uint actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(uint expected, uint actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(uint expected, uint actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(uint expected, uint actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(long expected, long actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(long expected, long actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(long expected, long actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(long expected, long actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(long expected, long actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(long expected, long actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(ulong expected, ulong actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(ulong expected, ulong actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(ulong expected, ulong actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(ulong expected, ulong actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(ulong expected, ulong actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(ulong expected, ulong actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(float expected, float actual)
    {
        if (enabled && Mathf.Abs(expected - actual) > Epsilon)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(float expected, float actual, string message)
    {
        if (enabled && Mathf.Abs(expected - actual) > Epsilon)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(float expected, float actual, string format, params object[] args)
    {
        if (enabled && Mathf.Abs(expected - actual) > Epsilon)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(float expected, float actual, string format, A0 arg0)
    {
        if (enabled && Mathf.Abs(expected - actual) > Epsilon)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(float expected, float actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && Mathf.Abs(expected - actual) > Epsilon)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(float expected, float actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && Mathf.Abs(expected - actual) > Epsilon)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(double expected, double actual)
    {
        if (enabled && Math.Abs(expected - actual) > Epsilon)
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", expected.ToString(), actual.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(double expected, double actual, string message)
    {
        if (enabled && Math.Abs(expected - actual) > Epsilon)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(double expected, double actual, string format, params object[] args)
    {
        if (enabled && Math.Abs(expected - actual) > Epsilon)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(double expected, double actual, string format, A0 arg0)
    {
        if (enabled && Math.Abs(expected - actual) > Epsilon)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(double expected, double actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && Math.Abs(expected - actual) > Epsilon)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(double expected, double actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && Math.Abs(expected - actual) > Epsilon)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(object expected, object actual)
    {
        if (enabled && !(expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper("Assertion failed: expected '{0}' but was '{1}'", assertUtils.ToString(expected), assertUtils.ToString(actual));
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(object expected, object actual, string message)
    {
        if (enabled && !(expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual(object expected, object actual, string format, params object[] args)
    {
        if (enabled && !(expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0>(object expected, object actual, string format, A0 arg0)
    {
        if (enabled && !(expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1>(object expected, object actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && !(expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreEqual<A0, A1, A2>(object expected, object actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && !(expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(bool expected, bool actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(bool expected, bool actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(bool expected, bool actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(bool expected, bool actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(bool expected, bool actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(bool expected, bool actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(byte expected, byte actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(byte expected, byte actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(byte expected, byte actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(byte expected, byte actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(byte expected, byte actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(byte expected, byte actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(sbyte expected, sbyte actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(sbyte expected, sbyte actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(sbyte expected, sbyte actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(sbyte expected, sbyte actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(sbyte expected, sbyte actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(sbyte expected, sbyte actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(short expected, short actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(short expected, short actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(short expected, short actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(short expected, short actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(short expected, short actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(short expected, short actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(ushort expected, ushort actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(ushort expected, ushort actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(ushort expected, ushort actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(ushort expected, ushort actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(ushort expected, ushort actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(ushort expected, ushort actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(char expected, char actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(char expected, char actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(char expected, char actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(char expected, char actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(char expected, char actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(char expected, char actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(int expected, int actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(int expected, int actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(int expected, int actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(int expected, int actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(int expected, int actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(int expected, int actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(uint expected, uint actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(uint expected, uint actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(uint expected, uint actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(uint expected, uint actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(uint expected, uint actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(uint expected, uint actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(long expected, long actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(long expected, long actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(long expected, long actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(long expected, long actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(long expected, long actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(long expected, long actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(ulong expected, ulong actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(ulong expected, ulong actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(ulong expected, ulong actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(ulong expected, ulong actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(ulong expected, ulong actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(ulong expected, ulong actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(float expected, float actual)
    {
        if (enabled && Mathf.Abs(expected - actual) < Epsilon)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(float expected, float actual, string message)
    {
        if (enabled && Mathf.Abs(expected - actual) < Epsilon)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(float expected, float actual, string format, params object[] args)
    {
        if (enabled && Mathf.Abs(expected - actual) < Epsilon)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(float expected, float actual, string format, A0 arg0)
    {
        if (enabled && Mathf.Abs(expected - actual) < Epsilon)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(float expected, float actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && Mathf.Abs(expected - actual) < Epsilon)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(float expected, float actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && Mathf.Abs(expected - actual) < Epsilon)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(double expected, double actual)
    {
        if (enabled && Math.Abs(expected - actual) < Epsilon)
            AssertHelper("Assertion failed: values are equal '{0}'", expected.ToString());
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(double expected, double actual, string message)
    {
        if (enabled && Math.Abs(expected - actual) < Epsilon)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(double expected, double actual, string format, params object[] args)
    {
        if (enabled && Math.Abs(expected - actual) < Epsilon)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(double expected, double actual, string format, A0 arg0)
    {
        if (enabled && Math.Abs(expected - actual) < Epsilon)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(double expected, double actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && Math.Abs(expected - actual) < Epsilon)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(double expected, double actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && Math.Abs(expected - actual) < Epsilon)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(object expected, object actual)
    {
        if (enabled && (expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper("Assertion failed: objects are equal '{0}'", assertUtils.ToString(expected));
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(object expected, object actual, string message)
    {
        if (enabled && (expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual(object expected, object actual, string format, params object[] args)
    {
        if (enabled && (expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0>(object expected, object actual, string format, A0 arg0)
    {
        if (enabled && (expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1>(object expected, object actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && (expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotEqual<A0, A1, A2>(object expected, object actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && (expected != null && actual != null && expected.Equals(actual) || expected == null && actual == null))
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreSame(object expected, object actual)
    {
        if (enabled && expected != actual)
            AssertHelper("Assertion failed: object references are not the same '{0}' but was '{1}'", assertUtils.ToString(expected), assertUtils.ToString(actual));
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreSame(object expected, object actual, string message)
    {
        if (enabled && expected != actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreSame(object expected, object actual, string format, params object[] args)
    {
        if (enabled && expected != actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreSame<A0>(object expected, object actual, string format, A0 arg0)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreSame<A0, A1>(object expected, object actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreSame<A0, A1, A2>(object expected, object actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected != actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotSame(object expected, object actual)
    {
        if (enabled && expected == actual)
            AssertHelper("Assertion failed: object references are the same '{0}'", assertUtils.ToString(expected));
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotSame(object expected, object actual, string message)
    {
        if (enabled && expected == actual)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotSame(object expected, object actual, string format, params object[] args)
    {
        if (enabled && expected == actual)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotSame<A0>(object expected, object actual, string format, A0 arg0)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotSame<A0, A1>(object expected, object actual, string format, A0 arg0, A1 arg1)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void AreNotSame<A0, A1, A2>(object expected, object actual, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && expected == actual)
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Range

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Range(int value, int lo, int hi)
    {
        if (enabled && (value < lo || value > hi))
        {
            AssertHelper("Value {0} is out of range [{1}..{2}]", value.ToString(), lo.ToString(), hi.ToString());
        }
    }

    #endregion

    #region Collections

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Contains<T>(T expected, ICollection<T> collection)
    {
        if (enabled && (collection == null || !collection.Contains(expected)))
        {
            if (collection == null)
                AssertHelper("Assertion failed: collection is null");
            else
                AssertHelper("Assertion failed: collection doesn't contain the item {0}", expected);
        }
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void NotContains<T>(T expected, ICollection<T> collection)
    {
        if (enabled && (collection != null && collection.Contains(expected)))
        {
            if (collection == null)
                AssertHelper("Assertion failed: collection is null");
            else
                AssertHelper("Assertion failed: collection contains the item {0}", expected);
        }
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<T>(ICollection<T> collection)
    {
        if (enabled && collection != null && collection.Count == 0)
            AssertHelper("Assertion failed: collection is null or not empty '{0}'", collection);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<T>(ICollection<T> collection, string message)
    {
        if (enabled && collection != null && collection.Count == 0)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<T>(ICollection<T> collection, string format, params object[] args)
    {
        if (enabled && collection != null && collection.Count == 0)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<T, A0>(ICollection<T> collection, string format, A0 arg0)
    {
        if (enabled && collection != null && collection.Count == 0)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<T, A0, A1>(ICollection<T> collection, string format, A0 arg0, A1 arg1)
    {
        if (enabled && collection != null && collection.Count == 0)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<T, A0, A1, A2>(ICollection<T> collection, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && collection != null && collection.Count == 0)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<T>(ICollection<T> collection)
    {
        if (enabled && collection != null && collection.Count != 0)
            AssertHelper("Assertion failed: collection is null or empty '{0}'", collection);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<T>(ICollection<T> collection, string message)
    {
        if (enabled && collection != null && collection.Count != 0)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<T>(ICollection<T> collection, string format, params object[] args)
    {
        if (enabled && collection != null && collection.Count != 0)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<T, A0>(ICollection<T> collection, string format, A0 arg0)
    {
        if (enabled && collection != null && collection.Count != 0)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<T, A0, A1>(ICollection<T> collection, string format, A0 arg0, A1 arg1)
    {
        if (enabled && collection != null && collection.Count != 0)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<T, A0, A1, A2>(ICollection<T> collection, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && collection != null && collection.Count != 0)
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Failures

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Fail()
    {
        if (enabled)
            AssertHelper("Assertion failed");
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Fail(string message)
    {
        if (enabled)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Fail(string format, params object[] args)
    {
        if (enabled)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Fail<A0>(string format, A0 arg0)
    {
        if (enabled)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Fail<A0, A1>(string format, A0 arg0, A1 arg1)
    {
        if (enabled)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Fail<A0, A1, A2>(string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled)
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Comparations

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Greater<T>(T a, T b) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) <= 0)
            AssertHelper("Assertion failed: '{0}' is not greater than '{1}'", a, b);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Greater<T>(T a, T b, string message) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) <= 0)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Greater<T>(T a, T b, string format, params object[] args) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) <= 0)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Greater<T, A0>(T a, T b, string format, A0 arg0) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) <= 0)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Greater<T, A0, A1>(T a, T b, string format, A0 arg0, A1 arg1) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) <= 0)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Greater<T, A0, A1, A2>(T a, T b, string format, A0 arg0, A1 arg1, A2 arg2) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) <= 0)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void GreaterOrEqual<T>(T a, T b) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) < 0)
            AssertHelper("Assertion failed: '{0}' is not greater or equal to '{1}'", a, b);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void GreaterOrEqual<T>(T a, T b, string message) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) < 0)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void GreaterOrEqual<T>(T a, T b, string format, params object[] args) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) < 0)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void GreaterOrEqual<T, A0>(T a, T b, string format, A0 arg0) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) < 0)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void GreaterOrEqual<T, A0, A1>(T a, T b, string format, A0 arg0, A1 arg1) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) < 0)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void GreaterOrEqual<T, A0, A1, A2>(T a, T b, string format, A0 arg0, A1 arg1, A2 arg2) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) < 0)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Less<T>(T a, T b) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) >= 0)
            AssertHelper("Assertion failed: '{0}' is not less than '{1}'", a, b);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Less<T>(T a, T b, string message) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) >= 0)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Less<T>(T a, T b, string format, params object[] args) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) >= 0)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Less<T, A0>(T a, T b, string format, A0 arg0) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) >= 0)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Less<T, A0, A1>(T a, T b, string format, A0 arg0, A1 arg1) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) >= 0)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void Less<T, A0, A1, A2>(T a, T b, string format, A0 arg0, A1 arg1, A2 arg2) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) >= 0)
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void LessOrEqual<T>(T a, T b) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) > 0)
            AssertHelper("Assertion failed: '{0}' is not less or equal to '{1}'", a, b);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void LessOrEqual<T>(T a, T b, string message) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) > 0)
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void LessOrEqual<T>(T a, T b, string format, params object[] args) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) > 0)
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void LessOrEqual<T, A0>(T a, T b, string format, A0 arg0) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) > 0)
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void LessOrEqual<T, A0, A1>(T a, T b, string format, A0 arg0, A1 arg1) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) > 0)
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void LessOrEqual<T, A0, A1, A2>(T a, T b, string format, A0 arg0, A1 arg1, A2 arg2) where T : IComparable<T>
    {
        if (enabled && a.CompareTo(b) > 0)
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Inheritance

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType(Type type, object o)
    {
        if (enabled && (type == null || !type.IsInstanceOfType(o)))
            AssertHelper("Assertion failed: expected type of '{0}' but was '{1}'", type, o != null ? o.GetType() : (Type)null);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType(Type type, object o, string message)
    {
        if (enabled && (type == null || !type.IsInstanceOfType(o)))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType(Type type, object o, string format, params object[] args)
    {
        if (enabled && (type == null || !type.IsInstanceOfType(o)))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<A0>(Type type, object o, string format, A0 arg0)
    {
        if (enabled && (type == null || !type.IsInstanceOfType(o)))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<A0, A1>(Type type, object o, string format, A0 arg0, A1 arg1)
    {
        if (enabled && (type == null || !type.IsInstanceOfType(o)))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<A0, A1, A2>(Type type, object o, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && (type == null || !type.IsInstanceOfType(o)))
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<T>(object o)
    {
        if (enabled && !(o is T))
            AssertHelper("Assertion failed: expected type of '{0}' but was '{1}'", typeof(T), o != null ? o.GetType() : (Type)null);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<T>(object o, string message)
    {
        if (enabled && !(o is T))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<T>(object o, string format, params object[] args)
    {
        if (enabled && !(o is T))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<T, A0>(object o, string format, A0 arg0)
    {
        if (enabled && !(o is T))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<T, A0, A1>(object o, string format, A0 arg0, A1 arg1)
    {
        if (enabled && !(o is T))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsInstanceOfType<T, A0, A1, A2>(object o, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && !(o is T))
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType(Type type, object o)
    {
        if (enabled && (type != null && type.IsInstanceOfType(o)))
            AssertHelper("Assertion failed: object '{0}' is subtype of '{1}'", type, o != null ? o.GetType() : (Type)null);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType(Type type, object o, string message)
    {
        if (enabled && (type != null && type.IsInstanceOfType(o)))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType(Type type, object o, string format, params object[] args)
    {
        if (enabled && (type != null && type.IsInstanceOfType(o)))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<A0>(Type type, object o, string format, A0 arg0)
    {
        if (enabled && (type != null && type.IsInstanceOfType(o)))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<A0, A1>(Type type, object o, string format, A0 arg0, A1 arg1)
    {
        if (enabled && (type != null && type.IsInstanceOfType(o)))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<A0, A1, A2>(Type type, object o, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && (type != null && type.IsInstanceOfType(o)))
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<T>(object o)
    {
        if (enabled && (o is T))
            AssertHelper("Assertion failed: object '{0}' is subtype of '{1}'", typeof(T), o != null ? o.GetType() : (Type)null);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<T>(object o, string message)
    {
        if (enabled && (o is T))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<T>(object o, string format, params object[] args)
    {
        if (enabled && (o is T))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<T, A0>(object o, string format, A0 arg0)
    {
        if (enabled && (o is T))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<T, A0, A1>(object o, string format, A0 arg0, A1 arg1)
    {
        if (enabled && (o is T))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotInstanceOfType<T, A0, A1, A2>(object o, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && (o is T))
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Strings

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty(string str)
    {
        if (enabled && !string.IsNullOrEmpty(str))
            AssertHelper("Assertion failed: string is not empty '{0}'", str);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty(string str, string message)
    {
        if (enabled && !string.IsNullOrEmpty(str))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty(string str, string format, params object[] args)
    {
        if (enabled && !string.IsNullOrEmpty(str))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<A0>(string str, string format, A0 arg0)
    {
        if (enabled && !string.IsNullOrEmpty(str))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<A0, A1>(string str, string format, A0 arg0, A1 arg1)
    {
        if (enabled && !string.IsNullOrEmpty(str))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsEmpty<A0, A1, A2>(string str, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && !string.IsNullOrEmpty(str))
            AssertHelper(format, arg0, arg1, arg2);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty(string str)
    {
        if (enabled && string.IsNullOrEmpty(str))
            AssertHelper("Assertion failed: string is null or empty '{0}'", str);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty(string str, string message)
    {
        if (enabled && string.IsNullOrEmpty(str))
            AssertHelper(message);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty(string str, string format, params object[] args)
    {
        if (enabled && string.IsNullOrEmpty(str))
            AssertHelper(format, args);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<A0>(string str, string format, A0 arg0)
    {
        if (enabled && string.IsNullOrEmpty(str))
            AssertHelper(format, arg0);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<A0, A1>(string str, string format, A0 arg0, A1 arg1)
    {
        if (enabled && string.IsNullOrEmpty(str))
            AssertHelper(format, arg0, arg1);
    }

    [System.Diagnostics.Conditional("ASSERTS_ENABLED")]
    public static void IsNotEmpty<A0, A1, A2>(string str, string format, A0 arg0, A1 arg1, A2 arg2)
    {
        if (enabled && string.IsNullOrEmpty(str))
            AssertHelper(format, arg0, arg1, arg2);
    }

    #endregion

    #region Helpers

    struct AssertStackTrace
    {
        public readonly string method;
        public readonly string stackTrace;

        public AssertStackTrace(string method, string stackTrace)
        {
            this.method = method;
            this.stackTrace = stackTrace;
        }
    }

    static void AssertHelper(string format, params object[] args)
    {
        try
        {
            string message = assertUtils.TryFormat(format, args);
            string stackTrace = assertUtils.ExtractStackTrace(3);
            
            if (assertImp != null)
            {
                assertImp.OnAssert(message, stackTrace);
            }

            if (onAssertCallback != null)
            {
                onAssertCallback(message, stackTrace);
            }

            SetYourBreakPointHere(); // set you breakpoint at this function and press Continue
        }
        catch (Exception e)
        {
            Debug.LogError("Error while handling assertion: " + e.Message);
        }
    }

    static void SetYourBreakPointHere()
    {
    }

    #endregion
}

#if UNITY_EDITOR

class EditorAsserts
{
    private static EditorAsserts instance;
    private AssertSettings settingsInstance;

    static EditorAsserts()
    {
        instance = new EditorAsserts();
    }

    public static AssertSettings settings
    {
        get
        {
            if (instance.settingsInstance == null)
            {
                instance.settingsInstance = new AssertSettings();
            }
            return instance.settingsInstance;
        }
    }
}

class EditorAssertImp : IAssertImp
{
    public void OnAssert(string message, string stackTrace)
    {
        string method = ExtractMethod(stackTrace);

        if (EditorAsserts.settings.ShouldIgnoreAssert(method))
        {
            return;
        }

        #if UNITY_ASSERTIONS
        UnityEngine.Assertions.Assert.IsTrue(false, message);
        #endif

        string title = "Assertion Failed";
        string fullMessage = message + "\n\n" + stackTrace;

        assertUtils.DisplayDialog(title, fullMessage,
            "Show",    delegate() { ShowAssertion(stackTrace); },
            "More...", delegate() { ShowMoreOptions(title, fullMessage, method); },
            "Ignore");
    }

    void ShowAssertion(string stackTrace)
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPaused = true;
        }

        SourceFileInfo info;
        if (assertUtils.TryExtractSourceFileInfo(stackTrace, out info))
        {
            assertUtils.OpenFileAtLineExternal(info.path, info.line);
        }
    }

    void ShowMoreOptions(string title, string message, string method)
    {
        assertUtils.DisplayDialog(title, message,
            "Continue", null,
            "Ignore all", delegate() { EditorAsserts.settings.IgnoreAllAsserts(method); },
            "Set breakpoint...", delegate() { SetBreakpoint(); }
        );
    }

    void SetBreakpoint()
    {
        while (true)
        {
            SourceFileInfo info;
            if (TryResolveBreakPointSourceFile(out info))
            {
                assertUtils.OpenFileAtLineExternal(info.path, info.line);
            }

            if (EditorUtility.DisplayDialog("Assertion Failed", "Press 'Continue' when done", "Continue", "Retry"))
            {
                break;
            }
        }
    }

    bool TryResolveBreakPointSourceFile(out SourceFileInfo info)
    {
        try
        {
            string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            int lineIndex = assertUtils.FindLineOfToken(currentFile, "SetYourBreakPointHere();");
            if (lineIndex != -1)
            {
                string path = assertUtils.AssetsRelativePath(currentFile);
                info = new SourceFileInfo(path, lineIndex + 1); // line number starts at 1
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Can't resolve break point location: " + e.Message);
        }

        info = default(SourceFileInfo);
        return false;
    }

    string ExtractMethod(string stackTrace)
    {
        int lineBreakIndex = stackTrace.IndexOf('\n');
        return lineBreakIndex != -1 ? stackTrace.Substring(0, lineBreakIndex) : stackTrace;
    }
}

class AssertSettings
{
    private HashSet<string> ignoredAsserts;

    public AssertSettings()
    {
        ignoredAsserts = new HashSet<string>();
    }

    public bool ShouldIgnoreAssert(string method)
    {
        return ignoredAsserts.Contains(method);
    }

    public void IgnoreAllAsserts(string method)
    {
        ignoredAsserts.Add(method);
    }
}

#endif // UNITY_EDITOR