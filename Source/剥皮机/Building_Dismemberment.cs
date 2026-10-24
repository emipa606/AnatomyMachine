using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace LingMod;

public class Building_Dismemberment : Building_CryptosleepCasket
{
    public readonly List<KeyValuePairsA> PairsAs = [];
    private WorkMode workMode = WorkMode.Modea; //默认是第一个工作模式
    public Pawn Pan => innerContainer[0] as Pawn; //寻取第一个内容物
    public List<BodyPartRecord> BodyParts => Pan.health.hediffSet.GetNotMissingParts().ToList();

    private List<BodyPartRecord> ModeCBodyguards
    {
        get
        {
            var bodies = new List<BodyPartRecord>();
            foreach (var item in PairsAs)
            {
                if (item.Value)
                {
                    bodies.Add(item.Key);
                }
            }

            return bodies;
        }
    }

    public override string GetInspectString()
    {
        var text = base.GetInspectString();
        string str;
        switch (workMode)
        {
            case WorkMode.Modea:
                str = "ZuiDaLiYi".Translate();
                break;
            case WorkMode.Modeb:
                str = "WanQuanShouGe".Translate();
                break;
            default:
                str = "ZiDingYi".Translate();
                break;
        }

        return $"{text}\n " + "WorkMode".Translate() + "\n " + str;
    } //显示工作模式

    public override IEnumerable<Gizmo> GetGizmos()
    {
        foreach (var m in base.GetGizmos())
        {
            yield return m;
        }

        yield return new Command_Action
        {
            defaultLabel = "NextMode".Translate(),
            defaultDesc = "NextModeDesc".Translate(),
            icon = DismembermentTieTu.XiaYiGe,

            action = delegate
            {
                switch (workMode)
                {
                    case WorkMode.Modea:
                        workMode = WorkMode.Modeb;
                        break;
                    case WorkMode.Modeb:
                        workMode = WorkMode.Modec;
                        break;
                    case WorkMode.Modec:
                        workMode = WorkMode.Modea;
                        break;
                }
            }
        }; //模式切换
        if (!innerContainer.Any)
        {
            yield break;
        }

        yield return new Command_Action //执行解剖
        {
            defaultLabel = "StartDismemberment".Translate(),
            defaultDesc = "StartDismembermentDesc".Translate(),
            icon = DismembermentTieTu.DongShou,

            action = delegate { boPi(Pan); }
        };
        if (workMode == WorkMode.Modec)
        {
            yield return new Command_Action //开启选单界面
            {
                defaultLabel = "ZiDingYiMain".Translate(),
                defaultDesc = "ZiDingYiMainDesc".Translate(),
                icon = DismembermentTieTu.WoXiangXiang,

                action = delegate { Find.WindowStack.Add(new ChooseBodypartWindow(this)); }
            };
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref workMode, "workMode");
    }

    private void boPi(Pawn pawn)
    {
        var bodyPartRecords = pawn.health.hediffSet.GetNotMissingParts().ToList();
        switch (workMode)
        {
            case WorkMode.Modea:
            {
                foreach (var part in bodyPartRecords)
                {
                    if (part.def.spawnThingOnRemoved == null ||
                        !pawn.health.hediffSet.GetNotMissingParts().Contains(part) || part.IsCorePart)
                    {
                        continue;
                    }

                    pawn.health.AddHediff(HediffDefOf.MissingBodyPart, part);

                    GenSpawn.Spawn(ThingMaker.MakeThing(part.def.spawnThingOnRemoved), InteractionCell, Map);
                }

                break;
            }
            case WorkMode.Modeb:
            {
                foreach (var part in bodyPartRecords)
                {
                    if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part) || part.IsCorePart)
                    {
                        continue;
                    }

                    pawn.health.AddHediff(HediffDefOf.MissingBodyPart, part);
                    if (part.def.spawnThingOnRemoved != null)
                    {
                        GenSpawn.Spawn(ThingMaker.MakeThing(part.def.spawnThingOnRemoved), InteractionCell, Map);
                    }
                }

                break;
            }
            case WorkMode.Modec:
            {
                foreach (var part in ModeCBodyguards)
                {
                    if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part) || part.IsCorePart)
                    {
                        continue;
                    }

                    pawn.health.AddHediff(HediffDefOf.MissingBodyPart, part);
                    if (part.def.spawnThingOnRemoved != null)
                    {
                        GenSpawn.Spawn(ThingMaker.MakeThing(part.def.spawnThingOnRemoved), InteractionCell, Map);
                    }
                }

                break;
            }
        }

        for (var i = 0; i < 10; i++)
        {
            FilthMaker.TryMakeFilth(InteractionCell, Map, ThingDefOf.Filth_Blood);
        }

        innerContainer.TryDropAll(InteractionCell, Map, ThingPlaceMode.Near);
    } //(!part.IsCorePart)避免只移除最大部件
}