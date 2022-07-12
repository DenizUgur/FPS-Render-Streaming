using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class GameSettings : MonoBehaviour
{
    // variable to hold the current frame number
    private int frameNo;
    // variable to hold max frame rate
    const int maxFrameRate = 60;
    // path for the log file containing frame data
    private string logFilePath = @"D:\Games\Unity\FPS-Render-Streaming\RL-data\stats.csv";
    private StringBuilder csv;

    // Quality Presets
    // Texture Quality [discrete integer range, the lower the better (full - half - quarter - eighth)]
    public int textureQuality = 1;
    // Level-of-Detail-Bias [continuous range, the higher the better]
    public int lodBias = 3;

    // Interval to update the render settings (by frames)
    const float updateSettingInterval = 2.0f;
    private bool updateSettingFlag = false;
    // Interval to record the logs (by frames)
    const float saveLogsInterval = 2.0f;
    private bool saveLogsFlag = false;

    // Helper function to synchronize time intervals
    int elapsedTime(float seconds){
            return (int) (seconds/Time.deltaTime);
        }

    // Start is called before the first frame update
    void Awake(){
        // Set Max frame rate 
        Application.targetFrameRate = maxFrameRate;
        // Disable Vsync
        QualitySettings.vSyncCount = 0;

        // Initialize quality settings
        // Texture Quality
        QualitySettings.masterTextureLimit = textureQuality;
        // LOD Bias
        QualitySettings.lodBias = lodBias;

    }
    void Start()
    {
        // Initialize the log file
        csv = new StringBuilder();
        var headers = $"{"frameNo"}, {"fps"}, {"batches"}, {"triangles"}, {"vertices"}, {"shadowCasters"}, {"renderTextureChanges"}, {"frameTime"}, {"renderTime"}, {"renderTextureCount"}, {"renderTextureBytes"}, {"usedTextureMemorySize"}, {"usedTextureCount"}, {"screenRes"}, {"screenBytes"}, {"vboTotal"}, {"vboTotalBytes"}, {"vboUploads"}, {"vboUploadBytes"}, {"ibUploads"}, {"ibUploadBytes"}, {"visibleSkinnedMeshes"}, {"drawCalls"}";
        csv.AppendLine(headers); 
       
    }

    // Update is called once per frame
    void Update()
    {
        // Update Quality (currently commented)
        if(frameNo % elapsedTime(updateSettingInterval) == 0){
            //textureQuality = XXXX;
            //lodBias = XXXX;
            //QualitySettings.masterTextureLimit = textureQuality;
            //QualitySettings.lodBias = lodBias;
        }

        // Record Logs
        var txt__  = frameNo.ToString();
        var txt00  = (1.0/Time.deltaTime).ToString();
        var txt01  = UnityEditor.UnityStats.batches.ToString();
        var txt02  = UnityEditor.UnityStats.triangles.ToString();
        var txt03  = UnityEditor.UnityStats.vertices.ToString();
        var txt04  = UnityEditor.UnityStats.shadowCasters.ToString();
        var txt05  = UnityEditor.UnityStats.renderTextureChanges.ToString();
        var txt06  = UnityEditor.UnityStats.frameTime.ToString();
        var txt07  = UnityEditor.UnityStats.renderTime.ToString();
        var txt08  = UnityEditor.UnityStats.renderTextureCount.ToString();
        var txt09  = UnityEditor.UnityStats.renderTextureBytes.ToString();
        var txt10  = UnityEditor.UnityStats.usedTextureMemorySize.ToString();
        var txt11  = UnityEditor.UnityStats.usedTextureCount.ToString();
        var txt12  = UnityEditor.UnityStats.screenRes;
        var txt13  = UnityEditor.UnityStats.screenBytes.ToString();
        var txt14  = UnityEditor.UnityStats.vboTotal.ToString();
        var txt15  = UnityEditor.UnityStats.vboTotalBytes.ToString();
        var txt16  = UnityEditor.UnityStats.vboUploads.ToString();
        var txt17  = UnityEditor.UnityStats.vboUploadBytes.ToString();
        var txt18  = UnityEditor.UnityStats.ibUploads.ToString();
        var txt19  = UnityEditor.UnityStats.ibUploadBytes.ToString();
        var txt20  = UnityEditor.UnityStats.visibleSkinnedMeshes.ToString();
        //var txt21  = UnityEditor.UnityStats.visibleAnimations.ToString();
        var txt21  = UnityEditor.UnityStats.drawCalls.ToString();

        var entryLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}", 
                txt__, txt00, txt01, txt02, txt03, txt04, txt05, txt06, txt07, txt08, txt09, txt10, txt11, txt12, txt13, txt14, txt15, txt16, 
                txt17, txt18, txt19, txt20, txt21);

        csv.AppendLine(entryLine);  

        if(frameNo % elapsedTime(saveLogsInterval) == 0){
        
            File.WriteAllText(logFilePath, csv.ToString());
            // if needed to exit the editor
            //EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        frameNo++;
        // Display current time in logs
        Debug.Log(Time.time);
        
    }
}
