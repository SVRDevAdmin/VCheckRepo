   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullW0clr-namespace:VCheckViewer.Views.Pages.Dashboard"VCheckViewer.Views.Pages.Dashboard  5      9    3VCheckViewer.Views.Pages.Dashboard.ScheduleTestList   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:VCheckViewer.Views.Pages.Dashboard     5        N� Title$  ScheduleTestList��5      .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��0��6   $��3��6   
���   �$��0q�6C   ._�5      �� $��0q�6   ._�5      �  5      �� 5      $��* �6(   .+�5      �� -   5        $��borderScheduledTest��5      $��0��68   $��10q�6E    �� Padding$ 
10,10,0,10q�6Q    K� ZIndex$ 23��6f   $��0q�6x   
������� �� CornerRadius$ 20��6�   ._�5      �� $
��Auto=�6'   .��5      �� -   5        $��icScheduledTest��5   +   $��0q�6B   $ 23��6M   
���   �$��0q�5   7   ��5   "   �� 5    &   .R�5!   *   � $��30,10,30,10q�6/    5"   .   �� 5#   2   $
��auto �6C   �� 5$   2   $��* �6C    5&   .   �� 5'   2   $
��auto �6@   �� 5(   2   $
��auto �6@   �� 5)   2   $
��auto �6@   �� 5*   2   $
��auto �6@   .+�5-   .   �� $��0��65   $��0��6E   $��2��6R   
���   �$��0q�6w   $��30��6�   $��0,0,10,0q�6�   ._�5.   2   �� $ 30��69   $��30��6K   
���   �$
��Left=�6i   ._�5/   6   �� $0��*../../../Content/Images/Icons/icon_dog.png��6<   $��20��6p   $��20��6{   50   3   51   /   �� 53   .   $��1��65   $��0��6E   ._�54   2   �� ._�55   6   �� $��	Noto Sans)�6@   $��12(�6W   $	��Top=�6e   $
��Bold"�6�   ��6}   �� *	PatientID+6�   56   3   57   /   �� 58   .   $��1��65   $��1��6E   $��1q�6R   ._�59   2   �� $��	Noto Sans)�6<   $��12(�6S   $	��Top=�6a   ��6y   �� *InchargePerson+6�   5:   /   �� 5;   .   $��1��65   $��2��6E   $��1q�6R   ._�5<   2   �� $
��Left=�69   $ 4��6T   ._�5=   6   �� $��	Noto Sans)�5>   P   $��12(�6g   $	��Top=�5?   P   $��Center=�6h   ��5=   @   �� *ScheduledDateTime+ �� StringFormat$ {0:h:mm tt MMMM dd yyyy}��5@   7   5A   3   5B   /   �� 5D   .   
���   �$	��0.5q�6L   $��3��6b   $��2��6o   $��0,10,0,0q�6�   5E   +   5F   '   5H      5I      5K      �� 5L      -   5        $��borderNoScheduledTest��5L      $��0��6:   $��10q�6G   $ 
10,10,0,10q�6S   $ 23��6h   
�������$ 20��6�   $y�	Collapsed=�6�   ._�5M      ��  �� TextAlignment$ Center=�6$   $��	Noto Sans)�6;   $��12(�6R   ��6`   ��  �� Path$$ General_Message_NoScheduleTest�   	Resources	 �� Source#	 [  5N      5O      5P      5Q      5S      5T      