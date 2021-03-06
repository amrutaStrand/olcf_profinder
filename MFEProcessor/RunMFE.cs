using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Agilent.MassSpectrometry.DataAnalysis;
using Agilent.MassSpectrometry.DataAnalysis.PFDBStorage;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;
using DataTypes;

namespace MFEProcessor
{
    public class MFE
    {
        private ProfinderLogic qualAppLogic = null;

        struct StrcuctFindCpdItem           // struct are used for parallel processing
        {
            public Agilent.MassSpectrometry.DataAnalysis.ICompound icompound;
            public Agilent.MassSpectrometry.DataAnalysis.ICpdDetails idetails;
            public Agilent.MassSpectrometry.DataAnalysis.IDataAccess idataaccess;
            public Agilent.MassSpectrometry.DataAnalysis.IParameterSet ipset;
            public Agilent.MassSpectrometry.DataAnalysis.IFindCompounds ifindcpd;
            public bool bheight;
        };
        public MFE()
        {
            
            qualAppLogic = new ProfinderLogic();
            
            
        }
        
        public List<string> AnalysisFiles {
            get;
            set;
        }

        public List<DataTypes.CompoundGroup> Execute()
        {
            if(AnalysisFiles.Count < 0)
            {
                Console.WriteLine("No Sample Files Found");
            }
            return GetCompoundGroups(AnalysisFiles);
        }
        private void ExecuteCommand(AppManager qualAppLogic, IQualCommand cmdQualCommand)
        {
            try
            {
                // Assembly.LoadFile(@"D:\.pool\Assembly\Bin\BaseTof.dll");
                IActionItemCollection actions = qualAppLogic.Invoke(cmdQualCommand) as IActionItemCollection;

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            
        }

        void OnAppProgressEvent(object sender, AppProgressEventArgs args)
        {
            for (int idx = 0; idx < args.AppProgressItems.Length; idx++)
            {
                AppProgressItem progress = args.AppProgressItems[idx];
                //m_messageReporter.ReportMessage(progress.PercentComplete.ToString()
                    //+ " : " + progress.StageMessage);
            }
        }
        private AnalysisMethodLoadOptions GetAnalysisMethodLoadOptions(string methodFile)
        {
            AnalysisMethodLoadOptions loadMethod = AnalysisMethodLoadOptions.LoadFromWorklist;
            if (methodFile == null)
                return loadMethod;
            CmdOpenDAMethod cmdOpenMethod = new CmdOpenDAMethod(qualAppLogic, methodFile);
            ExecuteCommand(qualAppLogic, cmdOpenMethod);
            loadMethod = AnalysisMethodLoadOptions.UseCurrentlyLoaded;
            return loadMethod;
        }
        public List<DataTypes.CompoundGroup> GetCompoundGroups(List<string> analysisFiles)   
        {
            try
            {
                qualAppLogic.AppExecutionMode = AppExecutionMode.WorkListAutomation;
                CmdInitializeApplication cmdInit = new CmdInitializeApplication(qualAppLogic);
                ExecuteCommand(qualAppLogic, cmdInit);

                QualFeatureConfig.InitRegistryFromAppConfig();
                qualAppLogic.AppProgressEvent += new AppProgressEventHandler(OnAppProgressEvent);
                IAppState appState = qualAppLogic as IAppState;

                //appState.ThreadErrorReporter = m_messageReporter;

                AnalysisMethodLoadOptions loadMethod = GetAnalysisMethodLoadOptions(null);
                //if (analysisFiles.Count > 0)
                //{
                    bool loadResults = false;
                    bool runLoadTimeScripts = false;

                    CmdOpenAnalysisFile cmdOpenAnalysis = new CmdOpenAnalysisFile
                                                                (qualAppLogic,
                                                                analysisFiles.ToArray(),
                                                                loadMethod,
                                                                loadResults,
                                                                runLoadTimeScripts);

                    ExecuteCommand(qualAppLogic, cmdOpenAnalysis);
                    
                    RunScript(qualAppLogic, analysisFiles);
                    IEnumerable<Agilent.MassSpectrometry.DataAnalysis.ICompoundGroup> cpdGroups = qualAppLogic.DataStore.CompoundGroups;
                    List<DataTypes.CompoundGroup> compoundGroups = new List<DataTypes.CompoundGroup>();
                    foreach (Agilent.MassSpectrometry.DataAnalysis.ICompoundGroup cpdGroup in cpdGroups)
                    {

                        DataTypes.CompoundGroup compoundGroup = getICompoundGroup(cpdGroup, analysisFiles) as DataTypes.CompoundGroup;
                        compoundGroups.Add(compoundGroup);
                    }
                Console.ReadKey();
            }
            catch (MSDAApplicationException msdaExep)
            {
                Console.WriteLine(msdaExep.Message);
            }
            catch (Exception e)
            {
                string analysisName = "";
                for (int analysisInd = 0; analysisInd < analysisFiles.Count; analysisInd++)
                {
                    if (analysisName == "")
                        analysisName = analysisFiles[analysisInd];
                    else
                        analysisName = analysisName + " , " + analysisFiles[analysisInd];
                }

                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        private DataTypes.CompoundGroup getICompoundGroup(Agilent.MassSpectrometry.DataAnalysis.ICompoundGroup cpdGroup, List<string> analysisFiles)
        {
            
            DataTypes.CompoundGroup compoundGroup = new DataTypes.CompoundGroup();
            if (cpdGroup == null)
            {
                return null;
            }

            compoundGroup.Group = cpdGroup.CompoundGroupName;
            compoundGroup.RTTgt = cpdGroup.TargetRetentionTime;
            compoundGroup.RTMed = cpdGroup.RetentionTimeMedian;
            compoundGroup.Found = cpdGroup.FrequencyFound;
            compoundGroup.Missed = cpdGroup.FrequencyMissed;
            //compoundGroup.ScoreTgtMax = cpdGroup.TgtScoreMax;
            //compoundGroup.TgtScorePctRSD = cpdGroup.TgtScorePctRSD;
            compoundGroup.ScoreMFEMax = cpdGroup.QScoreMax;
            compoundGroup.HeightMed = cpdGroup.HeightMedian;
            compoundGroup.MassAvg = cpdGroup.MassAverage;
            compoundGroup.RTAvg = cpdGroup.RetentionTimeAverage;
            compoundGroup.TargetMass = cpdGroup.TargetMass;
            compoundGroup.MassMedian = cpdGroup.MassMedian;
            compoundGroup.MassPpmRSD = cpdGroup.MassPpmRSD;
            compoundGroup.Saturated = cpdGroup.SaturationWarning;
            compoundGroup.RetentionTimeSpan = cpdGroup.RetentionTimeSpan;
            compoundGroup.SingleIonFeatures = cpdGroup.SingleIonFeatures;
            compoundGroup.RetentionTimeWidthAtBase = cpdGroup.RetentionTimeWidthAtBase;
            compoundGroup.AreaAvg = cpdGroup.AreaAverage;
            compoundGroup.HeightAvg = cpdGroup.HeightAverage;
            compoundGroup.VolumeAvg = cpdGroup.VolumeAverage;
            compoundGroup.RetentionTimeDifference = cpdGroup.RetentionTimeDifference;
            //TODO dnt know about sigma(A^2)
            compoundGroup.TimeSegment = cpdGroup.TimeSegment;

            var vals = cpdGroup.Values;
            IDictionary<string, DataTypes.ICompound> sampleWiseDict = new Dictionary<string, DataTypes.ICompound>();
            ConcurrentDictionary<int, ProfinderLogic.Strcuctplots> plotDict = getPlots(cpdGroup, analysisFiles);
            int ind = 0;
            foreach (var element in plotDict)
            {
                Agilent.MassSpectrometry.DataAnalysis.ICompound curCompound = vals.ElementAt(ind);
                DataTypes.ICompound compound = getCompound(curCompound, element);
                sampleWiseDict.Add(curCompound.DataFileName, compound);
                ind++;
            }
            compoundGroup.SampleWiseDataDictionary = sampleWiseDict;
            return compoundGroup;
            
        }

        private ConcurrentDictionary<int, ProfinderLogic.Strcuctplots> getPlots(Agilent.MassSpectrometry.DataAnalysis.ICompoundGroup cpdGroup, List<string> analysisFiles)
        {
            var plotDict = new ConcurrentDictionary<int, ProfinderLogic.Strcuctplots>();
            var dictAnalysisID = qualAppLogic.DataHeirarchy.AnalysisStore.Cast<DictionaryEntry>()
    .ToDictionary(kvp => kvp.Value as Analysis, kvp => (int)kvp.Key);
            if (!dictAnalysisID.Any())
                return plotDict;

            Agilent.MassSpectrometry.DataAnalysis.ICompound c = cpdGroup.First().Value;
            CpdDataObjectType plotTypes = CpdDataObjectType.MFECpdChromatogram | CpdDataObjectType.MFESpectrum;
            ConcurrentDictionary<int, StrcuctFindCpdItem> concurrentDict = new ConcurrentDictionary<int, StrcuctFindCpdItem>();
            List<string> sItems = new List<string> { };
            foreach (string analsisFilePath in analysisFiles)
            {
                StrcuctFindCpdItem strcItems;
                IFindCompounds findCpds = null;
                IParameterSet resultOptions = null;
                Agilent.MassSpectrometry.DataAnalysis.ICompound cpd;
                if (dictAnalysisID.FirstOrDefault(p => p.Key.FilePath == analsisFilePath).Key == null)
                    continue;
                var diAnalysis = dictAnalysisID.First(a => a.Key.FilePath == analsisFilePath);
                var analysis = diAnalysis.Key;
                var analysisID = diAnalysis.Value;
                bool flag = cpdGroup.TryGetValue(analsisFilePath, out cpd);
                if (!flag)
                    continue;
                IList<ICpdDetails> cpdDetailsList = cpd.CpdDetailsList;
                ICpdDetails cpdDetails = null;
                if ((cpdDetailsList.Count > 0) && (cpdDetailsList[0] != null))
                    cpdDetails = cpd.CpdDetailsList.First();
                if (cpdDetails == null && c.CpdMiningAlgorithm == CpdMiningAlgorithm.FindByMolecularFeature)
                {
                    MGDBAccessor DBAccessor = MGDBAccessor.GetMGDBAccessor();
                    string sKey = cpd.DataFileName + cpd.TargetID.ToString();
                    try
                    {
                        byte[] cpdDetailObj = DBAccessor.GetCompoundDetailItem(analysis.FilePath, sKey);
                        using (MemoryStream ms = new MemoryStream(cpdDetailObj))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            cpdDetails = (MFECpdDetails)bf.Deserialize(ms);
                        }
                    }
                    catch
                    {
                        continue;  // not a saved item
                    }
                    List<IParameterSet> parameterSets = new List<IParameterSet>();
                    parameterSets.Add(qualAppLogic.ProcessingParameters);
                    parameterSets.Add(qualAppLogic.ResultFilters);
                    parameterSets.Add(qualAppLogic.ChargeStateAssignment);
                    parameterSets.Add(qualAppLogic.TofPeakFinderPset);
                    parameterSets.Add(qualAppLogic.MSMSPeakFilter);
                    ((MFECpdDetails)cpdDetails).ChargeStateAssignmentPset = qualAppLogic.ChargeStateAssignment;
                    ((MFECpdDetails)cpdDetails).TofPeakFinderPset = qualAppLogic.TofPeakFinderPset;
                    ((MFECpdDetails)cpdDetails).MSMSPeakFilterPset = qualAppLogic.MSMSPeakFilter;
                    ((MFECpdDetails)cpdDetails).ParameterSets = parameterSets;
                }

                List<Type> algoTypeList = cpd.GetFindCompoundAlgorithmTypes();
                if (algoTypeList.Count > 0)
                {
                    Type algoType = cpd.GetFindCompoundAlgorithmTypes().First();
                    try
                    {
                        findCpds = new FindCpdsMassHunter(QualCommandBase.CreatePeakFinderForMassSpectrum());
                        resultOptions = qualAppLogic[QualDAMethod.ParamKeyMFEProcessing] as PSetMassHunterProcessing;
                    }
                    catch (Exception ex)
                    {
                        if (findCpds != null && qualAppLogic.OpenProject)  // no pset verification for open project
                        {
                            if (algoType == typeof(FindByIsotopologue))
                            {
                                resultOptions = qualAppLogic.GetParameterSetNoValidate(QualDAMethod.ParamKeyFindByIsotopologue);
                            }
                            else
                            {
                                resultOptions = qualAppLogic.GetParameterSetNoValidate(QualDAMethod.ParamKeyFindCompoundsFormula);
                            }
                        }
                        else throw ex;
                    }
                }
                var psetFbF = resultOptions as IPSetFindCpdsFormula;
                if (psetFbF != null)
                    psetFbF.PreferProfileRawSpectra = false;
                var psetMFE = resultOptions as IPSetMassHunterProcessing;
                if (psetMFE != null)
                    psetMFE.PreferProfileRawSpectra = false;

                strcItems.icompound = cpd;
                strcItems.idetails = cpdDetails;
                strcItems.ifindcpd = findCpds;
                strcItems.ipset = resultOptions;
                strcItems.idataaccess = analysis.DataAccess;
                strcItems.bheight = cpd.HasValue(ResultAttribute.Height);
                concurrentDict.TryAdd(analysisID, strcItems);
            }
            try
            {
                Parallel.ForEach(concurrentDict, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, strctItem =>
                {
                    StrcuctFindCpdItem myItems = (StrcuctFindCpdItem)strctItem.Value;
                    Agilent.MassSpectrometry.DataAnalysis.Qualitative.ProfinderLogic.Strcuctplots strcplots = new Agilent.MassSpectrometry.DataAnalysis.Qualitative.ProfinderLogic.Strcuctplots();
                    IFindCompounds findCpds = myItems.ifindcpd;
                    IParameterSet resultOptions = myItems.ipset;

                    if (myItems.idetails != null)
                    {
                        if (myItems.idetails is CpdDetailsFindByIsotopologue)
                        {
                            IPSetFindByIsotopologue psetFindIso = resultOptions as IPSetFindByIsotopologue;
                            if (psetFindIso != null && psetFindIso.PreferRawSpectrumDisplay)
                                plotTypes = CpdDataObjectType.MsChromatogram | CpdDataObjectType.MsSpectrum;
                            else
                                plotTypes = CpdDataObjectType.MsChromatogram | CpdDataObjectType.CleanedSpectrum;
                        }


                        var ifxData = findCpds.GetCpdDataObjects(myItems.icompound, myItems.idetails, plotTypes, myItems.idataaccess, resultOptions);
                        strcplots.ifxdata = ifxData;
                        List<IFXData> fxList = strcplots.ifxdata;
                        foreach (var chromItem in fxList)
                        {
                            //chromItem.RemoveCompoundInformation(); // label will use it
                            FXDataBase fxDatabase = chromItem as FXDataBase;
                            if ((fxDatabase != null) && (fxDatabase.AcquisitionMetaData != null))
                            {
                                fxDatabase.AcquisitionMetaData = null;
                            }
                        }
                        strcplots.bheight = myItems.icompound.HasValue(ResultAttribute.Height);
                        strcplots.cpdGroupNum = cpdGroup.CompoundGroupNumber;

                        plotDict[strctItem.Key] = strcplots;
                    }
                });
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }

            return plotDict;
        }

        private DataTypes.ICompound getCompound(Agilent.MassSpectrometry.DataAnalysis.ICompound curCompound, KeyValuePair<int, ProfinderLogic.Strcuctplots> element)
        {
            DataTypes.ICompound compound = new DataTypes.Compound();
            compound.RT = curCompound.RetentionTime;
            compound.Mass = curCompound.Mass;
            compound.ZCount = curCompound.ZCount;
            //Area is not implemented by Agilent.MassSpectrometry.DataAnalysis.ICompound
            compound.Ions = curCompound.IonCount;
            compound.Volume = curCompound.Volume;
            compound.FileName = curCompound.DataFileName;
            compound.Saturated = curCompound.SaturationWarning;
            compound.FileName = curCompound.DataFileName;
            compound.Width = curCompound.Width;

            int key = element.Key;

            ProfinderLogic.Strcuctplots plots = element.Value;
            List<IFXData> plotData = plots.ifxdata;
            foreach (IFXData data in plotData)
            {
                var xyStore = data.XYStore;
                if (data.Description is MSChromDescription)
                {
                    var chromatogram = getChromatogram(xyStore, data, curCompound.RetentionTime);
                    compound.Chromatogram = chromatogram;
                }
                if (data.Description is MassSpecDescription)
                {
                    var spectrum = getSpectrum(xyStore, data);
                    compound.Spectrum = spectrum;
                }
            }
            return compound;
        }

        private DataTypes.Spectrum getSpectrum(IFXStore xyStore, IFXData data)
        {
            DataTypes.Spectrum spectrum = new DataTypes.Spectrum();
            List<DataTypes.IPeak> peaks = new List<DataTypes.IPeak>();
            for (int i = 0; i < data.Count; i++)
            {
                DataTypes.IPeak peak = new DataTypes.Peak();
                peak.X = Math.Round(xyStore.GetX(i), 1);
                peak.Y = xyStore.GetY(i) / 100000;
                //peak.Name = vals.ElementAt(ind) as string;
                peaks.Add(peak);


            }
            spectrum.Peaks = peaks;
            spectrum.Name = data.Title;
            return spectrum;
        }

        private DataTypes.Chromatogram getChromatogram(IFXStore xyStore, IFXData data, double RT)
        {
            DataTypes.Chromatogram chromatogram = new DataTypes.Chromatogram();
            List<DataTypes.DataPoint> peaks = new List<DataTypes.DataPoint>();
            for (int i = 0; i < data.Count; i++)
            {
                DataTypes.DataPoint peak = new DataTypes.DataPoint();
                peak.X = Math.Round(xyStore.GetX(i), 1);
                peak.Y = xyStore.GetY(i) / 100000;
                //peak.Name = vals.ElementAt(ind) as string;
                peaks.Add(peak);


            }
            chromatogram.Data = peaks;
            chromatogram.Name = RT.ToString();
            chromatogram.Title = data.Title;
            return chromatogram;
        }

        private void RunScript(ProfinderLogic qualAppLogic, List<string> analysisFiles)
        {
            
            List<Analysis> Analyses = new List<Analysis>();
            foreach (string analysisPath in analysisFiles)
            {
                Analysis anlysis = qualAppLogic.DataHeirarchy.AnalysisStore.Values.Cast<Analysis>().First(a => a.FilePath == analysisPath);
                Analyses.Add(anlysis);
            }
            CmdFindByMFE cmdMFE1 = new CmdFindByMFE(qualAppLogic, MFEMode.RECURSIVE_STAGE_1, Analyses);
            ExecuteCommand(qualAppLogic, cmdMFE1);
            Console.WriteLine(" Find by MFE of " + MFEMode.RECURSIVE_STAGE_1 + " has completed");
            CmdRecursiveMFE rmfe = new CmdRecursiveMFE(qualAppLogic, Analyses);
            ExecuteCommand(qualAppLogic, rmfe);
            Console.WriteLine(" recursive mfe is done ");
            CmdFilterCompoundGroupsMFE cgMfe = new CmdFilterCompoundGroupsMFE(qualAppLogic, analysisFiles);
            ExecuteCommand(qualAppLogic, cgMfe);
            CmdFindByMFE cmdMFE2 = new CmdFindByMFE(qualAppLogic, MFEMode.RECURSIVE_STAGE_2, Analyses);
            ExecuteCommand(qualAppLogic, cmdMFE2);
            Console.WriteLine(" Find by MFE of " + MFEMode.RECURSIVE_STAGE_2 + " has completed");
            CmdFilterCompoundGroupsMFEPost cgpMfePost = new CmdFilterCompoundGroupsMFEPost(qualAppLogic, analysisFiles);
            ExecuteCommand(qualAppLogic, cgpMfePost);
            Console.WriteLine(" Filtering Done ");
            Console.WriteLine(" Compound Groups of datastore updated");
        }

        public void Run()
        {

        }
    }
}
