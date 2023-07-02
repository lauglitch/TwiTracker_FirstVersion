using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class CheckUnfollowers : MonoBehaviour
{
    List<string> followerList;
    List<string> followingList;
    List<string> dataToExport;
    string filePath;
    public ToastMessage toastMessage;
    public AudioSource successAudio;
    public AudioSource failureAudio;
    public GameObject sureToExitPopUp;

    void Start()
    {
        filePath = Application.dataPath + "/../dataExport.txt";
    }

    // Method called when user press "Export Data" button
    public void CheckUnmutuals()
    {
        ExtractDataFromOriginalFiles();
        CompareData();
        SaveListToFile(dataToExport, filePath);
    }

    // Saves all data from the original json files on lists
    void ExtractDataFromOriginalFiles()
    {
        string followerPath = Application.dataPath + "/../follower.js";
        string followingPath = Application.dataPath + "/../following.js";

        // Extract data from follower.js
        if (File.Exists(followerPath))
        {
            string followerString = File.ReadAllText(followerPath);
            followerList = new List<string>();

            int line = 5;
           
            while (ReadLineFromString(followerString, line) != null)
            {
                followerList.Add(ReadLineFromString(followerString, line));
                line = line + 6;
            }

            //PrintList(followerList);
        }
        else
        {
            toastMessage.ShowMessage("No se ha encontrado el archivo follower.js");
            failureAudio.Play();
            Debug.LogError("follower.js file not found!");
        }

        // Extract data from following.js
        if (File.Exists(followingPath))
        {
            string followingString = File.ReadAllText(followingPath);
            followingList = new List<string>();

            int line = 5;

            while (ReadLineFromString(followingString, line) != null)
            {
                followingList.Add(ReadLineFromString(followingString, line));
                line = line + 6;
            }

            //PrintList(followerList);
        }
        else
        {
            toastMessage.ShowMessage("No se ha encontrado el archivo following.js");
            failureAudio.Play();
            Debug.LogError("following.js file not found!");
        }
    }

    // Intersect and substract both lists to see people you´re following but they don´t follow you
    // Result = B - (A ∩ B)
    void CompareData()
    {
        List<string> intersectionList = new List<string>();

        intersectionList = GetIntersection<string>(followerList, followingList);
        dataToExport = SubtractLists(followingList, intersectionList);

        //Debug.Log("follower size: " + followerList.Count);
        //Debug.Log("following size: " + followingList.Count);
        //Debug.Log("intersection size: " + intersectionList.Count);
        //Debug.Log("result size: " + resultList.Count);
    }

    public string ReadLineFromString(string input, int lineNumber)
    {
        string[] lines = input.Split('\n');
        string result = "";

        if (lineNumber >= 1 && lineNumber <= lines.Length)
        {
            result = lines[lineNumber - 1];
            result = result.Substring(20);
            result = result.Substring(0, result.Length - 1);

            return result;
        }

        return null; // Line not found
    }

    public List<T> GetIntersection<T>(List<T> list1, List<T> list2)
    {
        return list1.Intersect(list2).ToList();
    }

    public List<T> SubtractLists<T>(List<T> list1, List<T> list2)
    {
        return list1.Except(list2).ToList();
    }

    public void PrintList(List<string> list)
    {
        foreach (string line in list)
            Debug.Log(line);
    }

    // Saves calculated data to a new file on the exe's root
    public void SaveListToFile(List<string> list, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string item in list)
                {
                    writer.WriteLine(item);
                }
            }
            toastMessage.ShowMessage("¡Archivo listo!");
            successAudio.Play();
            Debug.Log("File saved succesfully!");
        }
        catch (Exception ex)
        {
            toastMessage.ShowMessage("Error al guardar el archivo :(");
            failureAudio.Play();
            Debug.Log("Error trying to save file: " + ex.Message);
        }
    }

    public void openSureToExitPopu()
    {
        sureToExitPopUp.SetActive(true);
    }
    public void closeSureToExitPopu()
    {
        sureToExitPopUp.SetActive(false);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
