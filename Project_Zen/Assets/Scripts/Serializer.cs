using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Static class for serializing objects to a binary file
/// </summary>
public static class Serializer
{
    #region Public Methods

    /// <summary>
    /// Serializes the given object to the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    /// <param name="objectToSerialize">the object to serialize</param>
    public static void SerializeObject(string filename, System.Object objectToSerialize)
    {
        // Creates the file
        string filePath = Application.persistentDataPath + "/" + filename;
        Debug.Log(filePath);
        FileStream stream = new FileStream(filePath, FileMode.Create);

        // Serializes and saves the object
        BinaryFormatter bFormatter = new BinaryFormatter();
        bFormatter.Serialize(stream, objectToSerialize);
        stream.Close();
    }

    /// <summary>
    /// Deserializes and returns the object in the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    /// <returns>the deserialized object</returns>
    public static System.Object DeserializeObject(string filename)
    {
        System.Object deserializedObject;   // The object to be deserialized

        // Opens the file
        string filePath = Application.persistentDataPath + "/" + filename;
        FileStream stream = new FileStream(filePath, FileMode.Open);

        // Deserializes and returns the object
        BinaryFormatter binFormatter = new BinaryFormatter();
        deserializedObject = binFormatter.Deserialize(stream);
        stream.Close();
        return deserializedObject;
    }

    /// <summary>
    /// Deletes the selected file
    /// </summary>
    public static void DeleteFile(string filename)
    {
        // Gets the file path
        string filePath = Application.persistentDataPath + "/" + filename;

        // Deletes the file
        File.Delete(filePath);
    }

    #endregion
}
