using GpuScript;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class gsGps : gsGps_
{
  public override void RunComputeShader()
  {
    output = input + 1;
  }


  public float Avg_Earth_Radius_m = 6371000.0f;
  public float LatLongDist_m(float2 LatLong1, float2 LatLong2)
  {
    float2 Lat = radians(float2(LatLong1.x, LatLong2.x));
    float2 Long = radians(float2(LatLong1.y, LatLong2.y));
    return Avg_Earth_Radius_m * acos(cproduct(sin(Lat)) + cproduct(cos(Lat)) * extent(Long));
  }
  public float LatLongDist_m(float2 LatLong1, float altitude1, float2 LatLong2, float altitude2)
  {
    //return sqrt(sqr(LatLongDist_m(LatLong1, LatLong2)) + sqr(altitude2 - altitude1));
    float d2 = sqr(LatLongDist_m(LatLong1, LatLong2)) + sqr(altitude2 - altitude1);
    return d2 == 0 ? 0 : sqrt(d2);
  }

  //  ////https://www.summitpost.org/distance-to-the-center-of-the-earth/849764
  //  ////public float dist_to_earth_center(float lat, float elev)
  //  ////{
  //  ////  float a = 6378137.0f, b = 6356752.314f, L = radians(lat), Z = elev;
  //  ////}
  //  //public float LatLongDist_m(float2 LatLong1, float altitude1, float2 LatLong2, float altitude2)
  //  //{
  //  //  float2 Lat = radians(float2(LatLong1.x, LatLong2.x));
  //  //  float2 Long = radians(float2(LatLong1.y, LatLong2.y));
  //  //  //float dist_to_earth_center = tan(Lat) * 297257223563.0f / 298257223563.0f;
  //  //  float f = 0.996647189f;
  //  //  float2 tan_lat = tan(Lat);
  //  //  float2 tan_reduced_lat = tan(Lat) * f;
  //  //  float2 tan_geo_lat = tan_reduced_lat * f;
  //  //  float2 dist_to_earth_axis = 6378137.0f * cos(tan_reduced_lat);
  //  //  float2 dist_to_earth_center = dist_to_earth_axis / cos(tan_geo_lat) + float2(altitude1, altitude2);
  //  //  float d = middle(dist_to_earth_center) * acos(cproduct(sin(Lat)) + cproduct(cos(Lat)) * extent(Long));
  //  //  return sqrt(sqr(d) + sqr(altitude2 - altitude1));
  //  //}

  bool in_Start_GPS = false;
  //  //IEnumerator Start_GPS()
  //  //{
  //  //  in_Start_GPS = true;
  //  //  if (Input.location.status == LocationServiceStatus.Stopped)
  //  //  {
  //  //    if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) Permission.RequestUserPermission(Permission.FineLocation);
  //  //    if (!Input.location.isEnabledByUser) { status = $"Location not enabled, GPS accuracy = {accuracy}"; in_Start_GPS = activate = false; yield break; }

  //  //    Input.location.Start(accuracy.x);
  //  //    uint i;
  //  //    for (i = 0; Input.location.status == LocationServiceStatus.Initializing && i < 20; i++) { status = $"Initializing: {i} seconds"; yield return new WaitForSeconds(1); }
  //  //    if (i == 20) { status = "Timed out"; in_Start_GPS = activate = false; yield break; }
  //  //  }
  //  //  if (Input.location.status == LocationServiceStatus.Failed) { status = "Unable to determine device location"; in_Start_GPS = activate = false; yield break; }
  //  //  string filename = gpsFilename;
  //  //  status = $"GPS File = {filename}";
  //  //  if (filename.DoesNotExist())
  //  //    filename.WriteAllText($"Latitude\tLongitude\tAltitude_m\tDate_Time\tHorizontal_Accuracy_m\tVertical_Accuracy_m\tSpeed_kph");

  //  //  //while (Input.location.status == LocationServiceStatus.Running)
  //  //  //{
  //  //  //  var d = Input.location.lastData;
  //  //  //  bool moved = any(gpsPosition != float2(d.latitude, d.longitude)) || gpsAltitude != d.altitude;
  //  //  //  gpsPosition = float2(d.latitude, d.longitude);
  //  //  //  gpsAltitude = d.altitude;
  //  //  //  gpsAccuracy = float2(d.horizontalAccuracy, d.verticalAccuracy);
  //  //  //  date_time = d.timestamp.unix_date_utc().ToString("M/dd/yy h:mm:ss");
  //  //  //  //only append text if lat, long, or alt has changed
  //  //  //  if (any(gpsAccuracy > accuracy.yy)) moved = false;
  //  //  //  if (moved)
  //  //  //    filename.AppendText($"\n{d.latitude}\t{d.longitude}\t{d.altitude}\t{date_time}\t{d.horizontalAccuracy}\t{d.verticalAccuracy}");
  //  //  //  yield return new WaitForSeconds(1);
  //  //  //}

  //  //  string dateFormat = "M/dd/yy h:mm:ss";
  //  //  bool firstTime = true;
  //  //  while (Input.location.status == LocationServiceStatus.Running)
  //  //  {
  //  //    var d = Input.location.lastData;
  //  //    if (d.horizontalAccuracy < accuracy.y && d.verticalAccuracy < accuracy.y)
  //  //    {
  //  //      bool moved = any(gpsPosition != float2(d.latitude, d.longitude)) || gpsAltitude != d.altitude;
  //  //      DateTime time1 = d.timestamp.unix_date_utc();
  //  //      if (firstTime) speed = 0;
  //  //      else
  //  //      {
  //  //        //DateTime time0 = DateTime.ParseExact(date_time, dateFormat, null);
  //  //        //float secs = (float)((time1 - time0).TotalSeconds);
  //  //        //float m = LatLongDist_m(gpsPosition, gpsAltitude, float2(d.latitude, d.longitude), d.altitude);
  //  //        //float m_per_sec = m / secs;
  //  //        //UI_speed.mps = m_per_sec;
  //  //        speed = 0;
  //  //      }
  //  //      gpsPosition = float2(d.latitude, d.longitude);
  //  //      gpsAltitude = d.altitude;
  //  //      gpsAccuracy = float2(d.horizontalAccuracy, d.verticalAccuracy);
  //  //      date_time = time1.ToString("M/dd/yy h:mm:ss");
  //  //      //only append text if lat, long, or alt has changed
  //  //      if (any(gpsAccuracy > accuracy.yy)) moved = false;
  //  //      if (moved)
  //  //        filename.AppendText($"\n{d.latitude}\t{d.longitude}\t{d.altitude}\t{date_time}\t{d.horizontalAccuracy}\t{d.verticalAccuracy}\t{speed}");
  //  //      firstTime = false;
  //  //    }
  //  //    yield return new WaitForSeconds(1);
  //  //  }

  //  //  in_Start_GPS = activate = false;
  //  //}

  string timeFormat = "M/dd/yy h:mm:ss";

  IEnumerator Start_GPS()
  {
    in_Start_GPS = true;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    //print($"accuracy = {(accuracy / 1000).ToString("0", ", ")} m");
    //print($"accuracy = {(UI_accuracy.m).ToString("0", ", ")} m");
    if (gpsFilename.Exists())
    {
      var lines = gpsFilename.ReadAllLines();
      for (int i = 1; i < lines.Length; i++)
      {
        bool firstTime = i == 1;
        var items = lines[i].Split("\t");
        float lat = items[0].To_float(), lon = items[1].To_float(), alt = items[2].To_float(), vel = items[6].To_float();
        DateTime time1 = items[3].ToDate(timeFormat);

        //if (firstTime) speed = 0;
        //else
        //{
        //  DateTime time0 = date_time.ToDate(timeFormat);
        //  float secs = (float)((time1 - time0).TotalSeconds);
        //  if (secs > 0)
        //  {
        //    float m = LatLongDist_m(gpsPosition, gpsAltitude, float2(lat, lon), alt);
        //    float m_per_sec = m / secs;
        //    UI_speed.mps = m_per_sec;
        //  }
        //}

        gpsPosition = float2(lat, lon);
        gpsAltitude = alt;
        date_time = time1.ToString(timeFormat);
        yield return new WaitForSeconds(1);

      }
    }
#else
      if (Input.location.status == LocationServiceStatus.Stopped)
      {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) Permission.RequestUserPermission(Permission.FineLocation);
        if (!Input.location.isEnabledByUser) { status = $"Location not enabled, GPS accuracy = {accuracy}"; in_Start_GPS = activate = false; yield break; }

        Input.location.Start(accuracy.x);
        uint i;
        for (i = 0; Input.location.status == LocationServiceStatus.Initializing && i < 20; i++) { status = $"Initializing: {i} seconds"; yield return new WaitForSeconds(1); }
        if (i == 20) { status = "Timed out"; in_Start_GPS = activate = false; yield break; }
      }
      if (Input.location.status == LocationServiceStatus.Failed) { status = "Unable to determine device location"; in_Start_GPS = activate = false; yield break; }
      string filename = gpsFilename;
      status = $"GPS File = {filename}";
      //if (filename.DoesNotExist()) filename.WriteAllText($"Latitude\tLongitude\tAltitude_m\tDate_Time\tHorizontal_Accuracy_m\tVertical_Accuracy_m\tSpeed_kph");
      if (filename.DoesNotExist()) filename.WriteAllText($"Latitude\tLongitude\tAltitude_m\tDate_Time\tHorizontal_Accuracy_m\tVertical_Accuracy_m");
      //bool firstTime = true;
      while (Input.location.status == LocationServiceStatus.Running)
      {
        var d = Input.location.lastData;
        if (d.horizontalAccuracy < accuracy.y && d.verticalAccuracy < accuracy.y)
        {
          bool moved = any(gpsPosition != float2(d.latitude, d.longitude)) || gpsAltitude != d.altitude;
          DateTime time1 = d.timestamp.unix_date_utc();
          //if (firstTime) speed = 0;
          //else
          //{
          //  DateTime time0 = date_time.ToDate(timeFormat);
          //  float secs = (float)((time1 - time0).TotalSeconds);
          //  if (secs > 0)
          //  {
          //    float m = LatLongDist_m(gpsPosition, gpsAltitude, float2(d.latitude, d.longitude), d.altitude);
          //    float m_per_sec = m / secs;
          //    UI_speed.mps = m_per_sec;
          //  }
          //}
          gpsPosition = float2(d.latitude, d.longitude);
          gpsAltitude = d.altitude;
          gpsError = float2(d.horizontalAccuracy, d.verticalAccuracy);
          date_time = time1.ToString(timeFormat);
          if (any(gpsError > accuracy.yy)) moved = false;
          //if (moved) filename.AppendText($"\n{d.latitude}\t{d.longitude}\t{d.altitude}\t{date_time}\t{d.horizontalAccuracy}\t{d.verticalAccuracy}\t{speed}");
          if (moved) filename.AppendText($"\n{d.latitude}\t{d.longitude}\t{d.altitude}\t{date_time}\t{d.horizontalAccuracy}\t{d.verticalAccuracy}");
          //else
          //  speed = 0;
          //firstTime = false;
        }
        yield return new WaitForSeconds(1);
      }
#endif
    in_Start_GPS = activate = false;
  }

  public override void OnValueChanged_GS()
  {
    base.OnValueChanged_GS();
    if (UI_activate != null && UI_activate.Changed)
    {
      if (activate) { if (!in_Start_GPS) StartCoroutine(Start_GPS()); }
      else { status = "Deactivate LocationServices"; Input.location.Stop(); }
    }
  }

  //string gpsFilename { get => GetAndroidExternalFilesDir().ToPath() + "gps_lat_long_elev_time.txt"; }
  string gpsFilename
  {
    get
    {
      string path;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
      path = $"{Application.dataPath.BeforeLast("Assets")}{appName}/";
#else
        path = GetAndroidExternalFilesDir().ToPath();
#endif
      return path + "gps_data.txt";
    }
  }

  public override void Get_GPS_Data_Path()
  {
    string path = GetAndroidExternalFilesDir();
    print($"path = {path}");
    status = gpsFilename;
  }


  public string[] Get_Projects() { return new string[] { "Culverts", "Utilities", "Delaminations" }; }
}

