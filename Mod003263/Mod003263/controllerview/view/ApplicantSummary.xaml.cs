﻿using Mod003263.DBstuff;
using Mod003263.wpf.controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utils.Difference;

namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for ApplicantSummary.xaml
    /// </summary>
    public partial class ApplicantSummary : UserControl {

        private Dictionary<Applicant, ApplicantRow> applicantMap;
        private int levenshteinThreshold = 5;

        public ApplicantSummary() {
            this.applicantMap = new Dictionary<Applicant, ApplicantRow>();
            InitializeComponent();
        }

        private void searchTxt_TextChanged(object sender, TextChangedEventArgs e) {
            // Iterate through applicant map
            // Find levenshtein distance between each applicant and the query
            // Filter out those outside the threshold
            // Sort based upon the distance, exact matches should be separated and be at top

            String query = searchTxt?.Text;

            List<KeyValuePair<Applicant, ApplicantRow>> matches = this.applicantMap.Where(pair => pair.Key.Full_Name.Contains(query)).ToList();
            List<DistancedPair> distanced = new List<DistancedPair>();
            foreach (KeyValuePair<Applicant, ApplicantRow> pair in this.applicantMap.Where(pair => !matches.Contains(pair))) {
                String name = pair.Key.Full_Name;
                name = name.Substring(0, query.Length);
                int distance = StringDifferences.DamerauLevenshtein(query, name);
                if (distance < this.levenshteinThreshold)
                    distanced.Add(new DistancedPair{Distance=distance, Pair=pair});
            }

            distanced.Sort((a, b) => a.Distance.CompareTo(b.Distance));


            matches.ForEach(pair => {

            });
        }
    }

    public struct DistancedPair {
        public Int32 Distance { get; set; }
        public KeyValuePair<Applicant, ApplicantRow> Pair { get; set; }
    }
}