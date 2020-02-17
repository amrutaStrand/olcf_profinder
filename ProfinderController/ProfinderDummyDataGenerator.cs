using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;

namespace Agilent.OpenLab.ProfinderController
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class ProfinderDummyDataGenerator
    {
        Random random = new Random();

        IChromatogram GenerateChromatogram(string sampleName)
        {
            IChromatogram chromatogram = new Chromatogram();
            chromatogram.Name = sampleName;
            chromatogram.Title = sampleName;
            List<DataPoint> points = new List<DataPoint>();
            for (int i = 0; i < 10; i++)
            {
                DataPoint point = new DataPoint();
                point.X = random.NextDouble();
                point.Y = random.NextDouble();

                points.Add(point);
            }

            chromatogram.Data = points;

            return chromatogram;


        }

        ISpectrum GenerateSpectrum(string sampleName)
        {
            ISpectrum spectrum = new Spectrum();
            spectrum.Name = sampleName;
            List<IPeak> peaks = new List<IPeak>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                IPeak peak = new Peak();
                peak.X = random.NextDouble() * 100;
                peak.Y = random.NextDouble() * 400;
                peak.Name = sampleName;
                peaks.Add(peak);
            }
            spectrum.Peaks = peaks;
            return spectrum;
        }


        ICompound GenerateCompound(string sampleName)
        {
            ICompound compound = new Compound();
            compound.FileName = sampleName;
            compound.Mass = random.NextDouble();
            compound.RT = random.NextDouble();
            compound.Area = random.NextDouble();
            compound.Volume = random.NextDouble();
            compound.Saturated = true; ;
            compound.Width = random.NextDouble();
            compound.Ions = random.Next();
            compound.ZCount = random.Next();

            compound.Spectrum = GenerateSpectrum(sampleName);
            compound.Chromatogram = GenerateChromatogram(sampleName);

            return compound;
        }

        ICompoundGroup GenerateCompoundGroup(List<string> samples, int compoundGroupNumber)
        {

            ICompoundGroup compoundGroup = new CompoundGroup();
            compoundGroup.Group = "Group " + compoundGroupNumber;

            compoundGroup.RTTgt = random.NextDouble();
            compoundGroup.RTMed = random.NextDouble();
            compoundGroup.Found = random.Next();
            compoundGroup.Missed = random.Next();
            compoundGroup.ScoreMFEMax = random.NextDouble();
            compoundGroup.HeightMed = random.NextDouble();
            compoundGroup.MassAvg = random.NextDouble();
            compoundGroup.HeightAvg = random.NextDouble();
            compoundGroup.VolumeAvg = random.NextDouble();
            compoundGroup.TargetMass = random.NextDouble();
            compoundGroup.MassMedian = random.NextDouble();
            compoundGroup.MassPpmRSD = random.NextDouble();
            compoundGroup.MassPpmRSD = random.NextDouble();
            compoundGroup.RetentionTimeSpan = random.NextDouble();
            compoundGroup.RetentionTimeWidthAtBase = random.NextDouble();
            compoundGroup.TimeSegment = "Segment " + compoundGroupNumber;
            compoundGroup.RetentionTimeDifference = random.NextDouble();
            compoundGroup.SingleIonFeatures = random.Next();
            compoundGroup.Saturated = 10;

            IDictionary<string, ICompound> SampleWiseDataDictionary = new Dictionary<string, ICompound>();
            for (int i = 0; i < samples.Count; i++)
                SampleWiseDataDictionary.Add(samples.ElementAt(i), GenerateCompound(samples.ElementAt(i)));
            compoundGroup.SampleWiseDataDictionary = SampleWiseDataDictionary;

            return compoundGroup;
    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sampleCount"></param>
        /// <param name="numberOfCompoundGroups"></param>
        /// <returns></returns>

        public List<ICompoundGroup> GenerateDemoData(int sampleCount, int numberOfCompoundGroups)
        {
            
            List<ICompoundGroup> compoundGroups = new List<ICompoundGroup>();
            if (sampleCount <= 0) return compoundGroups;
            List<string> samples = new List<string>();
            for (int i=0; i<sampleCount; i++)
            {
                samples.Add("Sample " + (i+1));
            }

            for (int i=0; i<numberOfCompoundGroups; i++)
            {
                compoundGroups.Add(GenerateCompoundGroup(samples, i + 1));
            }

            return compoundGroups;
        } 

    }
}
