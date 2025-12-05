using UnityEditor.SceneManagement;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [HideInInspector] public GameManager manager;
    public Canvas canvas;
    public RectTransform mainTransform, bufferTransform;

    public float height;
    public Vector2 inGameMainPos, inGameBufferPos,
        inventoryMainPos, inventoryBufferPos,
        inGameMainScale, inGameBufferScale,
        inventoryMainScale, inventoryBufferScale;

    public float moveFrequency, moveDamping,
        scaleFrequency, scaleDamping;

    [HideInInspector]
    public Vector2 mainMoveVel = Vector2.zero, bufferMoveVel = Vector2.zero,
        mainScaleVel = Vector2.zero, bufferScaleVel = Vector2.zero;

    public SpringUtils.tDampedSpringMotionParams moveSpring = new(), scaleSpring = new();

    void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();

        mainTransform.anchoredPosition = DesiredMainPos();
        bufferTransform.anchoredPosition = DesiredBufferPos();
        mainTransform.localScale = DesiredMainScale();
        bufferTransform.localScale = DesiredBufferScale();
    }

    void Update()
    {
        Vector2 mainPos = mainTransform.anchoredPosition,
            bufferPos = bufferTransform.anchoredPosition,
            mainScale = mainTransform.localScale,
            bufferScale = bufferTransform.localScale,
            desiredMainPos = DesiredMainPos(),
            desiredBufferPos = DesiredBufferPos(),
            desiredMainScale = DesiredMainScale(),
            desiredBufferScale = DesiredBufferScale();

        SpringUtils.CalcDampedSpringMotionParams(ref moveSpring, Time.unscaledDeltaTime, moveFrequency, moveDamping);
        SpringUtils.CalcDampedSpringMotionParams(ref scaleSpring, Time.unscaledDeltaTime, scaleFrequency, scaleDamping);
        SpringUtils.UpdateDampedSpringMotion(
            ref mainPos.x, ref mainMoveVel.x, desiredMainPos.x, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref mainPos.y, ref mainMoveVel.y, desiredMainPos.y, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref bufferPos.x, ref bufferMoveVel.x, desiredBufferPos.x, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref bufferPos.y, ref bufferMoveVel.y, desiredBufferPos.y, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref mainScale.x, ref mainScaleVel.x, desiredMainScale.x, scaleSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref mainScale.y, ref mainScaleVel.y, desiredMainScale.y, scaleSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref bufferScale.x, ref bufferScaleVel.x,desiredBufferScale.x, scaleSpring);
        SpringUtils.UpdateDampedSpringMotion(
            ref bufferScale.y, ref bufferScaleVel.y, desiredBufferScale.y, scaleSpring);

        mainTransform.anchoredPosition = mainPos;
        bufferTransform.anchoredPosition = bufferPos;
        mainTransform.localScale = mainScale;
        bufferTransform.localScale = bufferScale;
    }

    public Vector2 Dimensions()
    {
        return new Vector2(height * Camera.main.aspect, height);
    }

    public Vector2 DesiredMainPos()
    {
        return manager.gameState == GameState.INVENTORY ? inventoryMainPos :
            -Dimensions() * 0.5f + inGameMainPos;
    }

    public Vector2 DesiredBufferPos()
    {
        return manager.gameState == GameState.INVENTORY ? inventoryBufferPos :
            -Dimensions() * 0.5f + inGameBufferPos;
    }

    public Vector2 DesiredMainScale()
    {
        return manager.gameState == GameState.INVENTORY ? inventoryMainScale :
            (manager.gameState == GameState.IN_GAME ||
            manager.gameState == GameState.END_SCREEN) ? inGameMainScale : Vector2.zero;
    }

    public Vector2 DesiredBufferScale()
    {
        return manager.gameState == GameState.INVENTORY ? inventoryBufferScale :
            (manager.gameState == GameState.IN_GAME ||
            manager.gameState == GameState.END_SCREEN) ? inGameBufferScale : Vector2.zero;
    }
}
