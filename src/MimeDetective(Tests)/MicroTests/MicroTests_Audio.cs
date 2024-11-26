using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MimeDetective.Tests;

[TestClass]
public class MicroTestsAudio : MicroTests {
    protected override string RelativeRoot() {
        return base.RelativeRoot() + @"Audio\";
    }

    [TestMethod]
    public void flacVLC_flac() {
        Test("flacVLC.flac");
    }

    [TestMethod]
    public void mp3ArchiveFrameSync_mp3() {
        Test("mp3ArchiveFrameSync.mp3");
    }

    [TestMethod]
    public void mp3ID3Test1_mp3() {
        Test("mp3ID3Test1.mp3");
    }

    [TestMethod]
    public void mp3ID3Test2_mp3() {
        Test("mp3ID3Test2.mp3");
    }

    [TestMethod]
    public void mp3VLCFrameSync_mp3() {
        Test("mp3VLCFrameSync.mp3");
    }

    [TestMethod]
    public void mp4WinVoiceApp_m4v() {
        Test("mp4WinVoiceApp.m4v");
    }

    [TestMethod]
    public void oggArchive_ogg() {
        Test("oggArchive.ogg");
    }

    [TestMethod]
    public void oggVLC_ogg() {
        Test("oggVLC.ogg");
    }

    [TestMethod]
    public void wavVLC_wav() {
        Test("wavVLC.wav");
    }

}