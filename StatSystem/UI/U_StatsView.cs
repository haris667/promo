using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class U_StatsView : MonoBehaviour
{
    public Text statsView;
    public Text effectsAndChangesView;
    public Transform dynamicCanvas;
    public Transform panel;

    private string statsStr;
    private string effectsAndChangesStr;

    private bool actived = false;
    private Stat[] _previousStats;

    private float disableTimerExtended = 3f;
    private float disableTimer = 3f;

    [Inject] U_AimTargetSystem aim;
    private void Start()
    {
        aim.onHitTarget += context =>
        {
            ViewStats(context.GetComponent<U_Unit>().Stats);
            U_BattleCalculator calc = context.GetComponent<U_BattleCalculator>();
            ViewEffectsAndChanges(calc.activeEffects, calc.statChanges);
            actived = true;

            SetActive(true);
            disableTimer = disableTimerExtended;
        };
    }
    private void Update()
    {
        disableTimer -= Time.deltaTime;

        if(disableTimer < 0)
            SetActive(false);
        
    }

    private void SetActive(bool active)
    {
        dynamicCanvas.gameObject.SetActive(active);
        panel.gameObject.SetActive(active);
    }
    public void ViewStats(Stat[] stats)
    {
        if (actived && !Enumerable.SequenceEqual<Stat>(_previousStats, stats))
            return;

        statsStr = string.Empty;

        for (int i = 0; i < stats.Length; i++)
        {
            statsStr += $"{stats[i].Type} - {stats[i].Current}\n"; 
        }
        _previousStats = stats;
        statsView.text = statsStr;
    }

    public void ViewEffectsAndChanges(List<EffectPack> activeEffects, List<StatChangePack> statChanges)
    {
        effectsAndChangesStr = string.Empty;

        for (int i = 0; i < activeEffects.Count; i++)
        {
            effectsAndChangesStr += $"{activeEffects[i].Type} - {activeEffects[i].RemainTime}\n";
        }

        for (int i = 0; i < statChanges.Count; i++)
        {
            effectsAndChangesStr += $"{statChanges[i].StatType} - {statChanges[i].RemainTime}\n";
        }

        effectsAndChangesView.text = effectsAndChangesStr;
    }
}
