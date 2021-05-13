using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Anchor
{
     public string identifier;
     public User user;
     public string model;
     public string userId;
}

[Serializable]
public class AnchorDTO
{
     public string identifier;
     public int visibility;
     public int communityId;
     public float size;
     public string model;
     public string userId;
     public User user;
     public List<TagInterraction> interactions;
     public DateTime CreationDate;
     public string lastUpdateDate;
     public float longitude;
     public float latitude;
     public int srid;
     public string pictureUrl;
}

