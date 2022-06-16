using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace QuickChargeDisplay
{
    internal static class ControlHelper
    {
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public static T FindChild<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject is T)
                return (T)dependencyObject;

            int childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject result = FindChild<T>(VisualTreeHelper.GetChild(dependencyObject, i));
                if (result != null)
                    return (T)result;
            }

            return null;
        }
    }
}
