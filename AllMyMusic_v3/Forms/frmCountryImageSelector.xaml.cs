﻿using System;
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
using System.Windows.Shapes;

using AllMyMusic_v3.ViewModel;

namespace AllMyMusic_v3
{
    public partial class frmCountryImageSelector : Window
    {
        private CountrySelectorViewModel _countrySelectorViewModel;

        public frmCountryImageSelector(CountrySelectorViewModel countrySelectorViewModel)
        {
            _countrySelectorViewModel = countrySelectorViewModel;

            InitializeComponent();

            DataContext = countrySelectorViewModel;
        }
    }
}