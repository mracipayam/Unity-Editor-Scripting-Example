
using System;
using UnityEditor;
using UnityEngine;

public class BasicObjectSpawner : EditorWindow
{
    string objectBaseName = "";
    int objectID = 1;
    GameObject objectToSpawn;
    float objectScale;
    float spawnRadius = 5f;

    Transform objectContainer;
    bool appendID;
    float minScaleValue=1f;
    float maxScaleValue=3f;
    float minScaleLimit = 0.5f;
    float maxScaleLimit = 3f;

    [MenuItem("Tools/Basic Object Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BasicObjectSpawner));
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Object",EditorStyles.boldLabel);
        EditorGUILayout.Space();
        objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn",objectToSpawn,typeof(GameObject),false) as GameObject;
        objectContainer = EditorGUILayout.ObjectField("Object Container",objectContainer,typeof(Transform),true) as Transform;
        EditorGUILayout.HelpBox("Object Container not required.", MessageType.None, false);

        EditorGUILayout.Space();
        objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);

        appendID = EditorGUILayout.BeginToggleGroup("Append Numerical ID", appendID);
        EditorGUI.indentLevel++;
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        EditorGUI.indentLevel--;
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        EditorGUILayout.Space();

        GUILayout.Label("Object Scale");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Min Limit :" + minScaleLimit);
        EditorGUILayout.MinMaxSlider(ref minScaleValue,ref maxScaleValue,minScaleLimit,maxScaleLimit);  
        EditorGUILayout.PrefixLabel("Max Limit :" + maxScaleLimit);
        EditorGUILayout.EndHorizontal(); 

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Min Value :" + minScaleValue.ToString());
        EditorGUILayout.LabelField("Max Value :" + maxScaleValue.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(objectToSpawn == null || objectBaseName == string.Empty || (objectContainer != null && EditorUtility.IsPersistent(objectContainer)));
        
        if(GUILayout.Button("Spawn Object"))
        {
            SpawnObject();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        if(objectToSpawn== null) { 
        
            EditorGUILayout.HelpBox("Place a Gameobject in the 'Prefab to Spawn' field.",MessageType.Warning);
        }
        if(objectBaseName== string.Empty) { 
        
            EditorGUILayout.HelpBox("Assing a base name to the object to be spawned.",MessageType.Warning);
        }
        if(objectBaseName== string.Empty) { 
        
            EditorGUILayout.HelpBox("Object Container must be a scene object.",MessageType.Warning);
        }



    }

    private void SpawnObject()
    {
        Vector2 spawnCircle = UnityEngine.Random.insideUnitSphere * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x,0,spawnCircle.y);
        objectScale = UnityEngine.Random.Range(minScaleValue, maxScaleValue);

        string objName = objectBaseName;
        if (appendID)
        {
            objName += objectID.ToString();
            objectID++;
        }

        GameObject newObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity, objectContainer);
        newObject.name = objName;
        newObject.transform.localScale = Vector3.one * objectScale;
    }
}
