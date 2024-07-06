﻿using UnityEditor;
using UnityEngine;

public class AudioSetting : AssetPostprocessor
{
    private void OnPostprocessAudio(AudioClip clip)
    {
        var importer = (AudioImporter) assetImporter;
        var setting = new AudioImporterSampleSettings();
        if (clip.length < 1)
            setting.loadType = AudioClipLoadType.DecompressOnLoad;
        else
            setting.loadType = AudioClipLoadType.Streaming;

        importer.preloadAudioData = false;
        importer.defaultSampleSettings = setting;
    }
}