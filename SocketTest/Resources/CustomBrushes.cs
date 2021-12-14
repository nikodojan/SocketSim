﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SocketTest.Resources
{
    public static class CustomBrushes
    {
        public static Border RoundBorder1 => RoundBlueBorder();

        public static Border ButtonBorder => RoundedButtonBorder();

        private static Border RoundBlueBorder()
        {
            Border border = new Border();
            border.BorderBrush = Brushes.SteelBlue;
            border.BorderThickness = new Thickness(1, 1, 1, 1);
            border.CornerRadius = new CornerRadius(6);
            
            return border;
        }

        private static Border RoundedButtonBorder()
        {
            Border border = new Border();
            border.BorderBrush = Brushes.SteelBlue;
            border.BorderThickness = new Thickness(1, 1, 1, 1);
            border.CornerRadius = new CornerRadius(6);

            return border;
        }


    }
}
