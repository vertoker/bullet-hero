using UnityEngine;
using UnityEditor;
using System;
public static class TypeUtilities
{
    public static bool IsNumbericType(this object obj)
    {
        switch (Type.GetTypeCode(obj.GetType()))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
    public static bool IsNumbericType(this Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}
public class NumericType// : IEquatable<>
{
    private object value;
    private Type type;
    public NumericType(object obj)
    {
        if (!obj.IsNumbericType())
        {
            throw new NumericTypeExpectedException("The type of object in the NumericType constructor must be numeric.");
        }
        value = obj;
        type = obj.GetType();
    }
    public object GetValue()
    {
        return value;
    }
    public void SetValue(object newValue)
    {
        value = newValue;
    }
    public bool Equals(NumericType other)
    {
        return this == other;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!(obj is NumericType))
            return GetValue() == obj;
        return Equals(obj);
    }
    public override int GetHashCode()
    {
        return GetValue().GetHashCode();
    }
    public override string ToString()
    {
        return GetValue().ToString();
    }
    public static bool operator <(NumericType left, NumericType right)
    {
        object leftValue = left.GetValue();
        object rightValue = right.GetValue();
        switch (Type.GetTypeCode(left.type))
        {
            case TypeCode.Byte:
                return (byte)leftValue < (byte)rightValue;
            case TypeCode.SByte:
                return (sbyte)leftValue < (sbyte)rightValue;
            case TypeCode.UInt16:
                return (ushort)leftValue < (ushort)rightValue;
            case TypeCode.UInt32:
                return (uint)leftValue < (uint)rightValue;
            case TypeCode.UInt64:
                return (ulong)leftValue < (ulong)rightValue;
            case TypeCode.Int16:
                return (short)leftValue < (short)rightValue;
            case TypeCode.Int32:
                return (int)leftValue < (int)rightValue;
            case TypeCode.Int64:
                return (long)leftValue < (long)rightValue;
            case TypeCode.Decimal:
                return (decimal)leftValue < (decimal)rightValue;
            case TypeCode.Double:
                return (double)leftValue < (double)rightValue;
            case TypeCode.Single:
                return (float)leftValue < (float)rightValue;
        }
        throw new NumericTypeExpectedException("Please compare valid numeric types.");
    }
    public static bool operator >(NumericType left, NumericType right)
    {
        object leftValue = left.GetValue();
        object rightValue = right.GetValue();
        switch (Type.GetTypeCode(left.type))
        {
            case TypeCode.Byte:
                return (byte)leftValue > (byte)rightValue;
            case TypeCode.SByte:
                return (sbyte)leftValue > (sbyte)rightValue;
            case TypeCode.UInt16:
                return (ushort)leftValue > (ushort)rightValue;
            case TypeCode.UInt32:
                return (uint)leftValue > (uint)rightValue;
            case TypeCode.UInt64:
                return (ulong)leftValue > (ulong)rightValue;
            case TypeCode.Int16:
                return (short)leftValue > (short)rightValue;
            case TypeCode.Int32:
                return (int)leftValue > (int)rightValue;
            case TypeCode.Int64:
                return (long)leftValue > (long)rightValue;
            case TypeCode.Decimal:
                return (decimal)leftValue > (decimal)rightValue;
            case TypeCode.Double:
                return (double)leftValue > (double)rightValue;
            case TypeCode.Single:
                return (float)leftValue > (float)rightValue;
        }
        throw new NumericTypeExpectedException("Please compare valid numeric types.");
    }
    public static bool operator ==(NumericType left, NumericType right)
    {
        return !(left > right) && !(left < right);
    }
    public static bool operator !=(NumericType left, NumericType right)
    {
        return !(left > right) || !(left < right);
    }
    public static bool operator <=(NumericType left, NumericType right)
    {
        return left == right || left < right;
    }
    public static bool operator >=(NumericType left, NumericType right)
    {
        return left == right || left > right;
    }
}
[Serializable]
public class NumericTypeExpectedException : Exception
{
    public NumericTypeExpectedException() { }
    public NumericTypeExpectedException(string message) : base(message) { }
    public NumericTypeExpectedException(string message, Exception inner) : base(message, inner) { }
    protected NumericTypeExpectedException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DrawIfAttribute : PropertyAttribute
{
    public string comparedPropertyName { get; private set; }
    public object comparedValue { get; private set; }
    public ComparisonType comparisonType { get; private set; }
    public DisablingType disablingType { get; private set; }

    /// <param name="comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
    /// <param name="comparedValue">The value the property is being compared to.</param>
    /// <param name="comparisonType">The type of comperison the values will be compared by.</param>
    /// <param name="disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
    public DrawIfAttribute(string comparedPropertyName, object comparedValue, ComparisonType comparisonType, DisablingType disablingType = DisablingType.DontDraw)
    {
        this.comparedPropertyName = comparedPropertyName;
        this.comparedValue = comparedValue;
        this.comparisonType = comparisonType;
        this.disablingType = disablingType;
    }
}
public enum ComparisonType
{
    Equals = 1,
    NotEqual = 2,
    GreaterThan = 3,
    SmallerThan = 4,
    SmallerOrEqual = 5,
    GreaterOrEqual = 6
}
public enum DisablingType
{
    ReadOnly = 2,
    DontDraw = 3
}
[CustomPropertyDrawer(typeof(DrawIfAttribute))]
public class DrawIfPropertyDrawer : PropertyDrawer
{
    // Reference to the attribute on the property.
    DrawIfAttribute drawIf;

    // Field that is being compared.
    SerializedProperty comparedField;

    // Height of the property.
    private float propertyHeight;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return propertyHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Set the global variables.
        drawIf = attribute as DrawIfAttribute;
        comparedField = property.serializedObject.FindProperty(drawIf.comparedPropertyName);

        // Get the value of the compared field.
        object comparedFieldValue = comparedField.objectReferenceValue;//.GetValue<object>();

        // References to the values as numeric types.
        NumericType numericComparedFieldValue = null;
        NumericType numericComparedValue = null;

        try
        {
            // Try to set the numeric types.
            numericComparedFieldValue = new NumericType(comparedFieldValue);
            numericComparedValue = new NumericType(drawIf.comparedValue);
        }
        catch (NumericTypeExpectedException)
        {
            // This place will only be reached if the type is not a numeric one. If the comparison type is not valid for the compared field type, log an error.
            if (drawIf.comparisonType != ComparisonType.Equals && drawIf.comparisonType != ComparisonType.NotEqual)
            {
                Debug.LogError("The only comparsion types available to type '" + comparedFieldValue.GetType() + "' are Equals and NotEqual. (On object '" + property.serializedObject.targetObject.name + "')");
                return;
            }
        }

        // Is the condition met? Should the field be drawn?
        bool conditionMet = false;

        // Compare the values to see if the condition is met.
        switch (drawIf.comparisonType)
        {
            case ComparisonType.Equals:
                if (comparedFieldValue.Equals(drawIf.comparedValue))
                    conditionMet = true;
                break;

            case ComparisonType.NotEqual:
                if (!comparedFieldValue.Equals(drawIf.comparedValue))
                    conditionMet = true;
                break;

            case ComparisonType.GreaterThan:
                if (numericComparedFieldValue > numericComparedValue)
                    conditionMet = true;
                break;

            case ComparisonType.SmallerThan:
                if (numericComparedFieldValue < numericComparedValue)
                    conditionMet = true;
                break;

            case ComparisonType.SmallerOrEqual:
                if (numericComparedFieldValue <= numericComparedValue)
                    conditionMet = true;
                break;

            case ComparisonType.GreaterOrEqual:
                if (numericComparedFieldValue >= numericComparedValue)
                    conditionMet = true;
                break;
        }

        // The height of the property should be defaulted to the default height.
        propertyHeight = base.GetPropertyHeight(property, label);

        // If the condition is met, simply draw the field. Else...
        if (conditionMet)
        {
            EditorGUI.PropertyField(position, property);
        }
        else
        {
            //...check if the disabling type is read only. If it is, draw it disabled, else, set the height to zero.
            if (drawIf.disablingType == DisablingType.ReadOnly)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property);
                GUI.enabled = true;
            }
            else
            {
                propertyHeight = 0f;
            }
        }
    }
}