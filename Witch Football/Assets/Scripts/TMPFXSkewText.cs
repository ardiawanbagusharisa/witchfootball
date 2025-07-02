using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPFXSkewText : MonoBehaviour
{
    public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0f, 0f), 
                                                            new Keyframe(0.05f, 0f), 
                                                            new Keyframe(0.15f, 1f), 
                                                            new Keyframe(0.25f, 0f),
                                                            new Keyframe(1f, 0f)); 
    private TMP_Text _TextComponent;
    public float CurveScale = 1.0f;
    public float ShearAmount = 1.0f;
    public float AnimationSpeed = 1.0f;

    void Awake(){
        _TextComponent = gameObject.GetComponent<TMP_Text>();
    }
    void Start(){
        StartCoroutine(WrapText());
        //StartCoroutine(AnimateText());
    }

    void Update() {
        //AnimatedText();
    }

    private AnimationCurve CopyAnimationCurve(AnimationCurve curve){
        AnimationCurve newCurve = new AnimationCurve();
        newCurve.keys = curve.keys;
        return newCurve;
    }
    IEnumerator WrapText(){
        VertexCurve.preWrapMode = WrapMode.Clamp;
        VertexCurve.postWrapMode = WrapMode.Clamp;

        Vector3[] vertices;
        Matrix4x4 matrix;
        _TextComponent.havePropertiesChanged = true;
        CurveScale *= 10;
        float oldCurveScale = CurveScale;
        float oldShearValue = ShearAmount;
        AnimationCurve oldCurve = CopyAnimationCurve(VertexCurve);

        while (true) {
            // <Edit later> Not working
            //VertexCurve = GetAnimatedText(VertexCurve);
            //Debug.Log(VertexCurve.keys[1].time);
            if(!_TextComponent.havePropertiesChanged && oldCurveScale == CurveScale && oldCurve[1].value == VertexCurve.keys[1].value && oldShearValue == ShearAmount){
                yield return null;
                continue;
            }
            oldCurveScale = CurveScale;
            oldCurve = CopyAnimationCurve(VertexCurve);
            oldShearValue = ShearAmount;
            // Generate the mesh and populate the textInfo with data we can use and manipulate.
            _TextComponent.ForceMeshUpdate();
            TMP_TextInfo textInfo = _TextComponent.textInfo;
            int characterCount = textInfo.characterCount;

            if(characterCount == 0) {
                continue;
            }

            float boundsMinX = _TextComponent.bounds.min.x;
            float boundsMaxX = _TextComponent.bounds.max.x;

            for(int i = 0; i < characterCount; i++){
                if(!textInfo.characterInfo[i].isVisible){
                    continue;
                }
                int vertexIndex = textInfo.characterInfo[i].vertexIndex; 
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                vertices = textInfo.meshInfo[materialIndex].vertices;
                // Compute the baseline mid point for each character
                Vector3 offsetToMidBaseline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, textInfo.characterInfo[i].baseLine);
                vertices[vertexIndex + 0] += -offsetToMidBaseline; 
                vertices[vertexIndex + 1] += -offsetToMidBaseline; 
                vertices[vertexIndex + 2] += -offsetToMidBaseline; 
                vertices[vertexIndex + 3] += -offsetToMidBaseline; 

                // Aply the shearing effect 
                float shearValue = ShearAmount * 0.01f; 
                Vector3 topShear = new Vector3(shearValue * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine), 0f, 0f);
                Vector3 bottomShear = new Vector3(shearValue * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y), 0f, 0f);

                vertices[vertexIndex + 0] += -bottomShear;
                vertices[vertexIndex + 1] += topShear;
                vertices[vertexIndex + 2] += topShear;
                vertices[vertexIndex + 3] += -bottomShear;

                // Compute the angle of rotation for each character based on the animation curve. 
                // Character's position relative to the bounds of the mesh
                float x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX);
                float x1 = x0 + 0.0001f;
                float y0 = VertexCurve.Evaluate(x0) * CurveScale;
                float y1 = VertexCurve.Evaluate(x1) * CurveScale;

                Vector3 horizontal = new Vector3(1, 0, 0);
                Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);
                float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                Vector3 cross = Vector3.Cross(horizontal, tangent);
                float angle = cross.z > 0 ? dot : 360 - dot;

                matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                vertices[vertexIndex + 0] += offsetToMidBaseline; 
                vertices[vertexIndex + 1] += offsetToMidBaseline; 
                vertices[vertexIndex + 2] += offsetToMidBaseline; 
                vertices[vertexIndex + 3] += offsetToMidBaseline; 
            }

            // Upload the mesh with the revised information
            _TextComponent.UpdateVertexData();
            //VertexCurve = CopyAnimationCurve(TempCurve);
            // WaitForSeconds(0.025f);
            yield return null;
        }
    }

    AnimationCurve GetAnimatedText(AnimationCurve animCurve){
        AnimationCurve newCurve = CopyAnimationCurve(animCurve);
        Keyframe[] keys = animCurve.keys;
        if(keys[keys.Length - 2].time >= keys[keys.Length - 1].time){
            for(int i = 1; i < keys.Length - 1; i++){
                keys[i].time = (i * 0.1f) - 0.05f;
            }
        } else {
            for(int i = 1; i < keys.Length - 1; i++){
                keys[i].time += AnimationSpeed * 0.05f;  //* Time.deltaTime;
            }
            Debug.Log(keys[1].time);        
        }
        newCurve.keys = keys;
        return newCurve;
    }

    void AnimatedText(){
        Debug.Log("Animated");
        Keyframe[] keys = VertexCurve.keys;
        if(keys[keys.Length - 2].time >= keys[keys.Length - 1].time){
            for(int i = 1; i < keys.Length - 1; i++){
                keys[i].time = (i * 0.1f) - 0.05f;
            }
        } else {
            for(int i = 1; i < keys.Length - 1; i++){
                keys[i].time += AnimationSpeed * 0.05f;  //* Time.deltaTime;
            }
            Debug.Log(keys[1].time);        
        }
        VertexCurve.keys = keys;
    }

    IEnumerator AnimateText(){
        Debug.Log("Animate Text");
        // There are 5 keyframes, 3 are the contents. 
        Keyframe[] keys = VertexCurve.keys;

        while(true){
            // If reached right edge then translate to left
            if(keys[keys.Length - 2].time >= keys[keys.Length - 1].time){
                for(int i = 1; i < keys.Length - 1; i++){
                    keys[i].time = (i * 0.1f) - 0.05f;
                }
            } else {
                for(int i = 1; i < keys.Length - 1; i++){
                    keys[i].time += AnimationSpeed * 0.05f;  //* Time.deltaTime;
                }
                yield return new WaitForSeconds(0.01f);
            }
            VertexCurve.keys = keys;
            
            yield return new WaitForSeconds(0.1f);
        }
        
        //yield return null;
    } 
}
