   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:VCheckViewer.Views.Pages.Dashboard"VCheckViewer.Views.Pages.Dashboard  5      9    3VCheckViewer.Views.Pages.Dashboard.NotificationList   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:VCheckViewer.Views.Pages.Dashboard     5        N� Title$  NotificationList��5      .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��0��6   $��3��6   
���   �$��0q�6C   ._�5      �� $��0q�6   ._�5      �  5      �� 5      $��* �6(   .+�5      �� -   5        $��borderNotification��5      $��0��67   $��10q�6D    �� Padding$ 	0,10,0,10q�6P    K� ZIndex$ 23��6d   $��0q�6v   
������� �� CornerRadius$ 20��6�   ._�5      �� $
��Auto=�6'   .��5      �� -   5        $��icNotification��5   +   $��0q�6A   $ 23��6L   ��5   "   �� 5   &   .R�5    *   � $��40,10,30,10q�6/    5!   .   �� 5"   2   $
��auto �6C   �� 5#   2   $��* �6C    5%   .   �� 5&   2   $
��auto �6@   �� 5'   2   $
��auto �6@   .+�5*   .   �� $��0��65   $��0��6E   
���   �$��0q�6f   $��30��6z   $��0,0,10,0q�6�   $	��Top=�6�   ._�5+   2   �� $ 30��69   $��30��6K   
���   �$
��Left=�6i   ._�5,   6   �� $0��*../../../Content/Images/Icons/icon_dog.png��6<   $��20��6p   $��20��6{   5-   3   5.   /   ]� 50   .   $��70��68   $��0��6D   $��1��6Q   .+�51   2   ]� $	��Top=�6<   .+�52   6   �� $
��Bold"�6c   $
��Left=�6u   ��6@   �� *NotificationTitle+6u   �� 53   6   $��Normal"�6]   $��Right=�6q    �� TextAlignment$ Right=�6�   ��6@   �� *CreatedDate+6�   54   3   ]� 55   2   $��Bottom=�6<   $��0,10,0,0q�6T   .+�56   6   �� $
��Wrap=�6@   $��11(�6y   $
��Left=�6�   $
��auto��6�   ��6T   �� *NotificationContent+6�   57   3   58   /   �� 5:   .   $��1��68   $��2��6E   
���   �$	��0.5q�6m   $��Bottom=�6�   5;   +   5<   '   5>      5?      5A      �� 5B      -   5        $��borderNoNotification��5B      $��0��69   $��10q�6F   $ 	0,10,0,10q�6R   $ 23��6f   
�������$ 20��6�   $y�	Collapsed=�6�   ._�5C      �� $ Center=�6$   $��	Noto Sans)�6;   $��12(�6R   ��6`   ��  �� Path$$ General_Message_NoNotification�   	Resources �� Source# [  5D      5E      5F      5G      5I      5J      