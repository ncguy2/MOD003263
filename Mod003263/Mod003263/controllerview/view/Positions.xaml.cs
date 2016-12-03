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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mod003263.db;
using Mod003263.events.io;
using Mod003263.interview;
using Mod003263.utils;

namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for Positions.xaml
    /// </summary>
    public partial class Positions : UserControl {

        private AvailablePosition pos;
        private List<AvailablePosition> positions;

        public Positions() {
            InitializeComponent();
            LoadPositions();
        }

        private void LoadPositions() {
            DatabaseAccessor.GetInstance().UsingAllPositions(list => {
                positions = list;
                RebuildList();
            });
        }

        private void RebuildList() {
            RebuildList(this.positions);
        }

        private void RebuildList(IEnumerable<AvailablePosition> positions) {
            lst_Positions.UnselectAll();
            lst_Positions.Items.Clear();
            foreach (AvailablePosition pos in positions)
                lst_Positions.Items.Add(pos);
            lst_Positions.Items.Add(new AvailablePosition{Id=-1, Position = "[New Position]", Seats=0});
        }

        private void SelectPosition(AvailablePosition pos) {
            this.pos = pos;
            if (this.pos == null) return;
            txt_Position.Text = this.pos.Position;
            spn_Seats.NUDTextBox.Text = this.pos.Seats+"";
        }

        private void Lst_Positions_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            AvailablePosition pos = lst_Positions.SelectedItem as AvailablePosition;
            if (pos == null) return;
            SelectPosition(pos);
        }

        private void CascadeData() {
            if (this.pos == null) return;
            this.pos.Position = txt_Position.Text;
            this.pos.Seats = spn_Seats.GetNumericValue();
        }

        private void Btn_SavePosition_OnClick(object sender, RoutedEventArgs e) {
            if (this.pos == null) return;
            CascadeData();
            new SavePositionEvent(this.pos).Fire();
            LoadPositions();
        }

        private void Txt_search_OnTextChanged(object sender, TextChangedEventArgs e) {
            string q = txt_search.Text.ToLower();
            if (q.Length <= 0) {
                RebuildList();
                return;
            }
            RebuildList(SmartSearch.Search(q, positions, AvailablePosition.GetEntities()));
        }
    }
}
