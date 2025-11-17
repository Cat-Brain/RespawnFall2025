using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

[ExecuteInEditMode]
public class InventoryInst : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler/*, ISelectable*/
{
    public InventoryController controller;
    public RectTransform rectTransform;
    public Image image;

    public InventoryItem item;

    [HideInInspector] public Vector2 desiredPos;
    [HideInInspector] public Vector2Int gridPos = -Vector2Int.one;
    [HideInInspector] public int index = -1;
    [HideInInspector] public Vector2 vel = Vector2.zero;

    [ProButton]
    public void Init()
    {
        Clean();
    }

    void Update()
    {
        if (!Application.isPlaying && image && item && item.sprite)
            image.sprite = item.sprite;
    }

    void LateUpdate()
    {
        if (!Application.isPlaying)
            return;

        Vector2 pos = rectTransform.anchoredPosition;
        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, controller.itemSpring);
        SpringUtils.UpdateDampedSpringMotion(ref pos.y, ref vel.y, desiredPos.y, controller.itemSpring);
        rectTransform.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        controller.RemoveFromInventory(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        desiredPos += controller.GlobalToLocalOffset(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        controller.AddToInventory(this);
    }

    public void CleanPivot()
    {
        rectTransform.pivot = Vector2.one / (2 * item.dimensions);
    }

    public void CleanDimensions()
    {
        rectTransform.localScale = CMath.Vector3XY_Z(item.dimensions, 1);
        rectTransform.sizeDelta = controller.cellWidth * Vector2.one;
    }

    public void Clean()
    {
        image.sprite = item.sprite;

        CleanPivot();
        CleanDimensions();
        
        if (gridPos == -Vector2Int.one)
        {
            index = controller.bufferItems.FindIndex((item) => item == this);
            controller.SnapBufferPosition(this);
        }
        else
        {
            index = controller.items.FindIndex((item) => item == this);
            controller.SnapInventoryPosition(this);
        }

        rectTransform.anchoredPosition = desiredPos;
    }
}