using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
   private static UI_Manager _instance;
   public static UI_Manager Instance => _ = _instance ? _instance : throw new ArgumentNullException(nameof(_instance));
   private void Awake() => _instance = this;
   
   [SerializeField] private Text _toolsCountText;
   public void UpdateToolsCountUI(int tools) => _toolsCountText.text = $"Tools: {tools}";
}
