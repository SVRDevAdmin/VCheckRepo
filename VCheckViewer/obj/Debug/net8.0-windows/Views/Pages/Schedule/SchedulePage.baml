   M S B A M L   `   `   `  ����G  CVCheckViewer, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullU/clr-namespace:VCheckViewer.Views.Pages.Schedule!VCheckViewer.Views.Pages.Schedule  5      4    .VCheckViewer.Views.Pages.Schedule.SchedulePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:VCheckViewer.Views.Pages.Schedule     5        N� Title$  SchedulePage��5	      .��5      � $��0, 0, 0, 10q�6    � RowDefinitions 5   
   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0q�6!   
���   �._�5      �  5      �� 5      $��2* �6$   �� 5      $��5* �6$   �� 5      $��5* �6$   �� 5      $��5* �6$    5      �� 5      $��* �6'   .+�5"      �� $��0��6   
���   �$��0q�6:   6P   �� 5$      $��1��6   
���   �$��0q�6:   ._�5%      �� $��0q�6   ._�5&      �  5'      �� 5(   "   $
��auto �60   �� 5)   "   $
��auto �60   �� 5*   "   $
��auto �60   .+�5-      �� $��0��6%   $��15, 8, 15, 20q�62    �� Padding$ 70q�6I    �� CornerRadius$ 7��6V      Themes_TestDoneBackground#���   6g   $��1q�6�   #���   6�   5/      �� 50      $��0��6%   $��70, 52q�62    K� ZIndex$ 21��6B   ._�51   "   �� -   5        $��lbTotalTest��51   ,   $��	Noto Sans)�6?   $��40(�6V   $
��Bold"�6d     Themes_SummaryFontColor#���  6v   06�   52      �� 53      $��0��6%   $��75, 98, 140, 30q�62   $ 22��6K   ._�54   "   �� $��	Noto Sans)�6,   $��16(�6C   #���  6�   ��6Q   ��  �� Path$ Schedule_Label_TestsDone�  	Resources �� Source# [ 6�   55      �� 56      $��0��6%   $��180, 38, 10, 0q�62   $ 20��6J   ._�57   "   �� $,��&../../../Content/Images/Img_Doctor.png��6(   $	��100��6X   58      59      5:      5;      �� 5=      $��2��6   
���   �$��0q�6:   ._�5>      �� $��0q�6   ._�5?      �  5@      �� 5A   "   $
��auto �60   .+�5D      �� $��0��6%   $��15, 8, 15, 20q�62   $ 70q�6I   $ 7��6V    ! Themes_TotalPatientBackground#���  6g   $��1q�6�   #���  6�   5E      �� 5F      $��0��6%   $��70, 52q�62   $ 21��6B   ._�5G   "   �� -   5        $��lbTotalPatients��5G   ,   $��	Noto Sans)�6C   $��40(�6Z   $
��Bold"�6h   #���  6z   06�   5H      �� 5I      $��0��6%   $��75, 98, 140, 30q�62   $ 22��6K   ._�5J   "   �� $��	Noto Sans)�6,   $��16(�6C   #���  6�   ��6Q   �� $ Schedule_Label_Patients�# [ 6�   5K      �� 5L      $��0��6%   $��180, 50, 10, 0q�62   $ 20��6J   ._�5M   "   �� $*��$../../../Content/Images/Img_Pets.png��6(   $	��110��6V   5N      5O      5P      5Q      �� 5S      $��3��6   
���   �$��0q�6:   ._�5T      �� $��0q�6   ._�5U      �  5V      �� 5W   "   $
��auto �60   .+�5Z      �� $��0��6%   $��15, 20, 15, 20q�62   $ 70q�6J   $ 7��6W     Themes_NewPatientBackground#���  6h   $��1q�6�   #���  6�   5[      �� 5\      $��0��6%   $��70, 60q�62   $ 21��6B   ._�5]   "   �� -   5        $��lbTotalNewPatients��5]   ,   $��	Noto Sans)�6F   $��40(�6]   $
��Bold"�6k   #���  6}   06�   5^      �� 5_      $��0��6%   $��75, 110, 140, 30q�62   $ 22��6L   ._�5`   "   �� $��	Noto Sans)�6,   $��16(�6C   #���  6�   ��6Q   �� $  Schedule_Label_NewPatients�# [ 6�   5a      �� 5b      $��0��6%   $��180, 65, 10, 0q�62   $ 20��6J   ._�5c   "   �� $*��$../../../Content/Images/Img_Pets.png��6(   $	��110��6V   5d      5e      5f      5g      5h      5i      �� 5l   
   $��1��6   $��0q�6!   
���   �._�5m      �  5n      �� 5o      $��2* �6$   �� 5p      $��5* �6$   �� 5q      $��5* �6$   �� 5r      $��5* �6$    5t      �� 5u      $��* �6'   .+�5x      �� $��0��6   
���   �$��0q�6:   ._�5y      ��   Themes_FontColor#���  6u   $��	Noto Sans)�6�   $��22(�6�   $
��Bold"�6�   $��85, 40, 0, 0q�6�   ��6    �� $! Schedule_Label_UpcomingTest�# [ 5z      5{      �� 5|      $��1��6   $��3��6&   
���   �$��0q�6K   ._�5}      �� $��0q�6   ._�5~      �  5      �� 5�   "   $��* �60   .+�5�      �� $��0��6%   $��10, 20, 10, 20q�62   $ 250, 255, 70, 255q�6J   $ 22��6f   $ 7��6x   $��1q�6�    ! Themes_UpcomingTestBackground#���  6�   #���  6�   5�      �� 5�      -   5        $��borderScheduledTest��5�   %   $��0��6@   $��25, 35, 25, 50q�6M   $ 20q�6e   $ 23��6r   $��0q�6�   ._�5�   "   �� $��-25, -30, -25, -25q�6/   $
��Auto=�6K   .��5�   &   �� -   5        $��icScheduledTest��5�   3   $��0q�6J   $ 23��6U   
���   �$��0q�5�   C   ��5�   *   �� 5�   .   .R�5�   2   � $��0q�67    5�   6   �� 5�   :   $��2* �6K   �� 5�   :   $��6* �6K   �� 5�   :   $��1* �6K    5�   6   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   .+�5�   6   �� $��0��6=   $��0��6M   $��5��6Z   6g   �� 5�   6   $��0��6=   $��1��6M   $��2��6Z   
���   �$��0q�6   $��30��6�   ._�5�   :   �� $ 30��6A   $��30��6S   
������ ._�5�   >   ��   Themes_DogIcon#���  6D   $��20��6n   $��20��6y   5�   ;   5�   7   �� 5�   6   $��0��6=   $��3��6M   $��2��6Z   #���  6k   $��
0, 0, 0, 1q�6�   6�   �� 5�   6   $��0��6=   $��0��6M   $��7��6Z   6g   �� 5�   6   $��1��6=   $��1��6M   
���   �$��0q�6n   $��15��6�   ._�5�   :   �� ._�5�   >   �� $��	Noto Sans)�6H   $��11(�6_   $��Center=�6m   #���  6�   �� 5�   >   �6B   �� *PatientIDString+ �� 5�   >   $
��Bold"�6]   �6B   �� *	PatientID+6]   5�   ?   5�   ;   5�   7   �� 5�   6   $��1��6=   $��2��6M   
���   �$��0q�6n   $��15��6�   ._�5�   :   �� $��	Noto Sans)�6D   $��11(�6[   $��Center=�6i   #���  6�   ��6�   �� *InchargePerson+6�   5�   7   �� 5�   6   $��1��6=   $��3��6M   
���   �$��0q�6n   $��25��6�   ._�5�   :   �� #���  6�   $��	Noto Sans)�6�   $��11(�6�   $��Center=�6�   ��6D   �� $$ Schedule_Label_ViewSchedulePMS�# [ 6  5�   7   �� 5�   6   $��1��6=   $��4��6M   #���  6Z   $��
0, 0, 0, 1q�6�   $��45��6�   ._�5�   :   �� $��70��6A   $��30��6L   ���#C2E2F5$
��Left=�6m   $ 4��6�   ._�5�   >   �� ���#1e76fb$��	Noto Sans)�5�   H   $��11(�6_   $
��Bold"�6m   $��Center=�5�   H   $��Center=�6c   ��5�   H   �� *ScheduledDateTime+ �� StringFormat$ {0:h:mm tt}��5�   ?   5�   ;   5�   7   �� 5�   6   $��2��6=   $��0��6M   $��5��6Z   
���   �$��0q�6   $	��Top=�6�   ._�5�   :   ��   Themes_ArrowRight#���  6@   $��15��6m   $��15��6x   $��0, 10q�6�   $y�	Collapsed=�6�   5�   7   5�   3   5�   /   5�   '   5�   #   5�      �� 5�      -   5        $��borderNoScheduledTest��5�   %   $��0��6B   $��25, 35, 25, 50q�6O   $ 30q�6g   $ 23��6t   $y�	Collapsed=�6�   ._�5�   "   �� #���  6�   $��50, 0, 20, 0q�6�   $��	Noto Sans)�6�   $��12(�6�   ��6,   �� $, &Schedule_Label_NoScheduleTestAvailable�# [ 6�   5�      5�      5�      5�      5�      5�      �� 5�   
   $��2��6   $��0q�6!   
���   �._�5�      �  5�      �� 5�      $��2* �6$   �� 5�      $��5* �6$   �� 5�      $��5* �6$   �� 5�      $��5* �6$    5�      �� 5�      $��* �6'   .+�5�      �� $��0��6   
���   �$��0q�6:   ._�5�      �� #���  6z   $��	Noto Sans)�6�   $��22(�6�   $
��Bold"�6�   $��85, 40, 0, 0q�6�   ��6    �� $&  Schedule_Label_RecentTestResults�# [ 5�      5�      �� 5�      $��1��6   $��3��6&   
���   �$��0q�6K   ._�5�      �� $��0q�6   ._�5�      �  5�      �� 5�   "   $��* �60   .+�5�      �� $��0��6%   $��10, 8, 10, 20q�62   $ 250, 255, 70, 255q�6I   $ 22��6e   $ 7��6w   $��1q�6�    	 Themes_RecentTestBackground#��� 	 6�   #��� 	 6�   5�      �� 5�      -   5        $��borderTestsCompleted��5�   %   $��0��6A   $��25, 24, 25, 50q�6N   $ 20q�6f   $ 23��6s   $��0q�6�   ._�5�   "   �� $��-25, -30, -25, -25q�6/   $
��Auto=�6K   .��5�   &   �� -   5        $��icTestResult��5�   3   $��0q�6G   $ 23��6R   
���   �$��0q�5�   C   ��5�   *   �� 5�   .   .R�5�   2   � $��0q�67    5�   6   �� 5�   :   $��2* �6K   �� 5�   :   $��6* �6K   �� 5�   :   $��1* �6K    5�   6   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   �� 5�   :   $
��auto �6H   �� 5   :   $
��auto �6H   .+�5  6   �� $��0��6=   $��0��6M   $��5��6Z   6g   �� 5  6   $��0��6=   $��1��6M   $��2��6Z   
���   �$��0q�6   $��30��6�   ._�5  :   �� $ 30��6A   $��30��6S   
������ ._�5  >   �� #���  6D   $��20��6n   $��20��6y   5	  ;   5
  7   �� 5  6   $��0��6=   $��3��6M   $��2��6Z   #���  6k   $��
0, 0, 0, 1q�6�   6�   �� 5  6   $��0��6=   $��0��6M   $��7��6Z   6g   �� 5  6   $��1��6=   $��1��6M   
���   �$��0q�6n   $��15��6�   ._�5  :   �� ._�5  >   �� $��	Noto Sans)�6H   $��11(�6_   $��Center=�6m   #���  6�   �� 5  >   �6B   �� *PatientIDString+ �� 5  >   $
��Bold"�6]   �6B   �� *	PatientID+6]   5  ?   5  ;   5  7   �� 5  6   $��1��6=   $��2��6M   
���   �$��0q�6n   $��15��6�   ._�5  :   �� $��	Noto Sans)�6D   $��11(�6[   $��Center=�6i   #���  6�   ��6�   �� *InchargePerson+6�   5  7   �� 5  6   $��1��6=   $��3��6M   
���   �$��0q�6n   $��25��6�   ._�5  :   �� #���  5  D   $��	Noto Sans)�6t   $��11(�6�   $��Center=�6�   ��5  D   �� $ Schedule_Label_ViewReport�# [ 5  �   5  7   �� 5  6   $��1��6=   $��4��6M   #���  6Z   $��
0, 0, 0, 1q�6�   $��45��6�   ._�5  :   �� $��70��6A   $��30��6L   
������ $
��Left=�6q   $ 4��6�   ._�5   >   �� ���#57baa5$��	Noto Sans)�5!  H   $��11(�6_   $
��Bold"�6m   $��Center=�5"  H   $��Center=�6c   ��5   H   �� *TestResultStatus+5#  ?   5$  ;   5%  7   �� 5(  6   $��2��6=   $��0��6M   $��5��6Z   
���   �$��0q�6   $	��Top=�6�   ._�5*  :   �� #���  6@   $��15��6m   $��15��6x   $��0, 10q�6�   $y�	Collapsed=�6�   5+  7   5,  3   5-  /   5/  '   50  #   52     �� 53     -	   5        $��borderNoTestsCompleted��53  %   $��0��6C   $��25, 35, 25, 50q�6P   $ 30q�6h   $ 23��6u   $y�	Collapsed=�6�   ._�54  "   �� #���  6�   $��50, 0, 20, 0q�6�   $��	Noto Sans)�6�   $��12(�6�   ��6,   �� $* $Schedule_Label_NoTestResultAvailable�# [ 6�   55     56     57     58     59     5:     5;     5<     