using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using TMPro;
public class WobblyRainbowText : MonoBehaviour
{
    public Gradient textGradient;
    public float gradientSpeed;
    public TMP_Text textComponent;
    private float _totalTime;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ColorChange());
        StartCoroutine(TextEffect());
        
    }

    private IEnumerator TextEffect()
    {
        while (GameManager.Instance.state == GameManager.GameState.MainMenu)
        {
            textComponent.ForceMeshUpdate();
            var textInfo = textComponent.textInfo;
            Color32 color0 = textComponent.color;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;
                var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                var newVertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                int vertexIndex = charInfo.vertexIndex;
                for (int j = 0; j < 4; j++)
                {
                    var orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] =
                        orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
                    var offset = i / textInfo.characterCount;
                    color0 = textGradient.Evaluate((_totalTime + offset) % 1f);  
                    _totalTime += Time.deltaTime;
                    newVertexColors[vertexIndex + j] = color0;
                    
                    
                }
            }
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                textComponent.UpdateGeometry(meshInfo.mesh, i);
                textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                
            }
            yield return new WaitForSeconds(gradientSpeed); //force update part making problem when both of the effects work at the same time look on it
            yield return null;
        }
    }

    private IEnumerator ColorChange()
    {
        while (GameManager.Instance.state == GameManager.GameState.MainMenu)
        {
            textComponent.ForceMeshUpdate();
            var textInfo = textComponent.textInfo;
            Color32 color0 = textComponent.color;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;
                var newVertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                int vertexIndex = charInfo.vertexIndex;
                
                for (int j = 0; j < 4; j++)
                {
                    var offset = i / textInfo.characterCount;
                    color0 = textGradient.Evaluate((_totalTime + offset) % 1f);  
                    _totalTime += Time.deltaTime;
                    newVertexColors[vertexIndex + j] = color0;
                    yield return new WaitForSeconds(gradientSpeed);
                    textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                }
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
