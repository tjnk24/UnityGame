using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(Damager))]
public class DamagerEditor : Editor
{
    static BoxBoundsHandle BoxBoundsHandle = new BoxBoundsHandle();
    static Color EnabledColor = Color.green + Color.grey;

    //сериализуемые свойства, в которые можно записать значения для отображения в инспекторе
    SerializedProperty damageProp;
    SerializedProperty offsetProp;
    SerializedProperty sizeProp;
    SerializedProperty OnDamageableHitProp;
    SerializedProperty OnNonDamageableHitProp;
    SerializedProperty hittableLayersProp;

    bool spriteFlip;

    //если выделен, то...
    private void OnEnable()
    {
        //найти свойства в привязанном через CustomEditor классе
        damageProp = serializedObject.FindProperty("damage");
        offsetProp = serializedObject.FindProperty("offset");
        sizeProp = serializedObject.FindProperty("size");
        hittableLayersProp = serializedObject.FindProperty("hittableLayers");
        OnDamageableHitProp = serializedObject.FindProperty("OnDamageableHit");
        OnNonDamageableHitProp = serializedObject.FindProperty("OnNonDamageableHit");
    }

    //то, что отображается в инспекторе
    public override void OnInspectorGUI()
    {
        //обновлять сериализуемые объекты в режиме реального времени
        serializedObject.Update();

        //вывести в инспектор поля с заданными значениями
        EditorGUILayout.PropertyField(damageProp);
        EditorGUILayout.PropertyField(offsetProp);
        EditorGUILayout.PropertyField(sizeProp);
        EditorGUILayout.PropertyField(hittableLayersProp);
        EditorGUILayout.PropertyField(OnDamageableHitProp);
        EditorGUILayout.PropertyField(OnNonDamageableHitProp);

        //применять обновления сериализуемых объектов
        serializedObject.ApplyModifiedProperties();
    }

    //то, что отображается на сцене в редакторе
    private void OnSceneGUI()
    {
        Damager damager = (Damager)target;

        //скрыть, если скрипт Damager выключен
        if (!damager.enabled)
            return;

        Matrix4x4 handleMatrix = damager.transform.localToWorldMatrix;
        //handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
        //handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        //handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, damager.transform.position.z));

        //отрисовка квадрата здесь
        using (new Handles.DrawingScope(handleMatrix))
        {

            ////флипаем квадрат для удобства и наглядности
            if (damager.spriteFlip)
            {
                BoxBoundsHandle.center = new Vector3(-damager.offset.x, damager.offset.y);
                BoxBoundsHandle.size = new Vector3(-damager.size.x, damager.size.y);
            }
            else
            {
                //задаём значения
                BoxBoundsHandle.center = damager.offset;
                BoxBoundsHandle.size = damager.size;
            }

            //задаём цвет
            BoxBoundsHandle.SetColor(EnabledColor);

            //начало проверки состояния GUI.changed
            EditorGUI.BeginChangeCheck();

            

            //отрисовываем
            BoxBoundsHandle.DrawHandle();

            
            //если состояние GUI.changed внутри этого блока кода равно true, то..
            if (EditorGUI.EndChangeCheck())
            {
                //без этой штуки undo/redo работают неполноценно и криво, она пишет все изменения объекта после себя
                Undo.RecordObject(damager, "Modify Damager");

                //собственно, применяем изменения к значениям
                damager.size = BoxBoundsHandle.size;
                damager.offset = BoxBoundsHandle.center;
            }
        }


    }
}
