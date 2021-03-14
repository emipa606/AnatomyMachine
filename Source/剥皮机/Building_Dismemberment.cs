using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace LingMod
{
    public class Building_Dismemberment : Building_CryptosleepCasket
    {
        public readonly List<KeyValuePairsA> pairsAs = new();
        private WorkMode workMode = WorkMode.Modea; //默认是第一个工作模式
        public Pawn Pan => innerContainer.First() as Pawn; //寻取第一个内容物
        public List<BodyPartRecord> bodyParts => Pan.health.hediffSet.GetNotMissingParts().ToList();

        private List<BodyPartRecord> modeCBodyparts
        {
            get
            {
                var bodies = new List<BodyPartRecord>();
                foreach (var item in pairsAs)
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

            return text + "\n " + "WorkMode".Translate() + "\n " + str;
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

                action = delegate { BoPi(Pan); }
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

        private void BoPi(Pawn pawn)
        {
            /*foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
            {
                if (hediff.def.spawnThingOnRemoved != null)
                {
                    GenSpawn.Spawn(ThingMaker.MakeThing(hediff.def.spawnThingOnRemoved), this.InteractionCell, this.Map);
                    pawn.health.RemoveHediff(hediff);
                }
            }*/
            var Allpart = pawn.health.hediffSet.GetNotMissingParts().ToList();
            if (workMode == WorkMode.Modea)
            {
                foreach (var part in Allpart)
                {
                    if (part.def.spawnThingOnRemoved == null ||
                        !pawn.health.hediffSet.GetNotMissingParts().Contains(part) || part.IsCorePart)
                    {
                        continue;
                    }

                    pawn.health.AddHediff(HediffDefOf.MissingBodyPart, part);

                    GenSpawn.Spawn(ThingMaker.MakeThing(part.def.spawnThingOnRemoved), InteractionCell, Map);
                }
            }

            if (workMode == WorkMode.Modeb)
            {
                foreach (var part in Allpart)
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
            }

            if (workMode == WorkMode.Modec)
            {
                foreach (var part in modeCBodyparts)
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
            }

            for (var i = 0; i < 10; i++)
            {
                FilthMaker.TryMakeFilth(InteractionCell, Map, ThingDefOf.Filth_Blood);
            }

            innerContainer.TryDropAll(InteractionCell, Map, ThingPlaceMode.Near);
        } //(!part.IsCorePart)避免只移除最大部件
    }
}