   M S B A M L   `   `   `  ����M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:QuickChargeDisplay.ControlQuickChargeDisplay.Control  5      K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  5      7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      %    QuickChargeDisplay.ChargingPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      <control(clr-namespace:QuickChargeDisplay.Control     5      @	Converter*clr-namespace:QuickChargeDisplay.Converter     5      2local clr-namespace:QuickChargeDisplay     5      -   5          N� Title$  ChargingPage��5      $	��800��6   $	��480��6&    � 	Resources 5
      ;   5QuickChargeDisplay.Converter.DoublePrecisionConverter   doublePrecisionConverter��%   &	          5   
   6-   5       N� 
Background 5      �� 5   
    �� ImageSource$0 *image\RAPIDAS-R_Monitor\05\충전중11.png��6   .��5      � .+�5   
   (   "QuickChargeDisplay.ExtendedBinding     Source 6    ��  �� Path$ ChargingSoc�   QuickChargeDisplay.QCDV   Charging �� Source# Z    Target 5       �� $ 
SocPercent�	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType"
 N�.   (QuickChargeDisplay.Control.AnimatedImage  5   
   -   5          Name$ 	BackImage��5       .   (QuickChargeDisplay.Control.AnimatedImage   	MouseDown$ BackImage_MouseDown��6X   $��3��63   $��2��6G   6X   �� 5   
   $E��?image\04_충전중화면\배터리화면\자동차아이콘.png��6   $
��Fill=�69    K� ZIndex$ 3��6H   $��180,78,500,281q�6Y   �� 5   
   -   5         ��Name$	 p10��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $#��84.378,55.398,682.622,261.602q�6L   �� 5   
   -   5        $	 p20��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��113.986,55.398,651.014,261.602q�6L   �� 5   
   -   5        $	 p30��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��144.496,55.398,620.504,261.602q�6L   �� 5   
   -   5        $	 p40��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��174.842,55.398,590.158,261.602q�6L   �� 5   
   -   5        $	 p50��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��205.352,55.398,559.648,261.602q�6L   �� 5   
   -   5        $	 p60��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��234.764,55.398,530.236,261.602q�6L   �� 5   
   -	   5        $	 p70��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��265.274,55.398,499.726,261.602q�6L   �� 5   
   -
   5        $	 p80��5      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��295.784,55.398,469.216,261.602q�6L   �� 5    
   -   5        $	 p90��5       $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $$��326.294,55.398,438.706,261.602q�6L   �� 5!   
   -   5        $
 p100��5!      $.��(image\RAPIDAS-R_Monitor\05\충전바.png��6   $��357,55,408,262q�6J   �� 5"   
   -   5         ��Name$ ChargeDevice��5"      
������ $	 100��6=    �� Left$	 307��6P   
 �� Top$	 268��6b   $��71,42,350,250q�6s   6�   �� 5#   
   -   5        $��SetPower��5#   \   $��0��6   $��79��6   $
��Left=�6(   $��144,290.872,0,0q�6C   $	��Top=�6l   $	��120��6�   ���	#FFFBF8F8$��50(�6�   $��Right=�6�   ��5$      �� 5%      5'      �� 5(   
   -   5        $��	Txttimemm��5(   �   $��0��6   $��50(�6   ���	#FFFBF8F8$��79��6A   $
��Left=�6M   $��Right=�6h   $��603,290.308,0,0q�6�   $	��Top=�6�   $��89��6�   ��5)      �� 5*      5,      �� 5-   
   -   5        $��chargingSoc��5-   �   $	��0.0��6   $��60(�6   ���	#FF0000E1$��97��6C   $
��Left=�6O   $��Right=�6j   $��603,159.872,0,0q�6�   $	��Top=�6�   $	��111��6�   ��5.      �� 5/      51      �� 54   
   -   5         ��Name$
 Info��54      $��BMS Info��6!   
���   �$��15(�55      $��0,0,371.746,368q�6+    J� RenderTransformOrigin$ 0.486,0.963&�6D   $
��Bold"�6h   y�54   3   �� *	Visible+ �� Mode$ TwoWay=�   BmsInfo# Z  �� UpdateSourceTrigger$ PropertyChanged=�.��56      ��  �� Orientation$ Vertical=�6   $��10,10,10,10q�60   .+�5;      �� $ 
Horizontal=�6   .+�5<      �� $��13(�6    $��74.233��6.    �� TextAlignment$ Right=�6=   $
��Left=�6S   �� 6o   $�
Output V :��6s   6�   �� 5=      -   5        $��OutputVoltage��5=       $��13(�65   $��80��6C   $��Right=�6N   ��6j   �� $ 
OutVoltage�# Z $ TwoWay=�$ PropertyChanged=��� 5>      $��13(�6    $	��120��6.   $ Right=�6:   �� 6Q   $�
Output A :��6U   6j   �� 5?      -   5        $��OutputCurrent��5?       $��13(�65   $��97.003��6C   ��6R   �� $ 	OutAmpare�# Z $ TwoWay=�$ PropertyChanged=�5@      �� 5A      $��2��6   $ 
Horizontal=�6(   
�������6A   �� 5C      $ 
Horizontal=�6   .+�5D      �� $��13(�6    $��74.233��6.   $ Right=�6=   $
��Left=�6S   �� 6o   $�
Target V :��6s   6�   �� 5E      -   5        $��TargetVoltage��5E       $��13(�65   $��80��6C   $��Right=�6N   ��6j   �� $ TargetVoltage�# Z $ TwoWay=�$ PropertyChanged=��� 5F      $��13(�6    $	��120��6.   $ Right=�6:   �� 6Q   $�
Target A :��6U   6j   �� 5G      -   5        $��TargetCurrent��5G       $��13(�65   $��97.003��6C   ��6R   �� $ TargetAmpare�# Z $ TwoWay=�$ PropertyChanged=�5H      �� 5I      $��2��6   $ 
Horizontal=�6(   
�������6A   �� 5K      $ 
Horizontal=�6   .+�5L      �� $��13(�6    $��85��6.   $ Right=�69   $
��Left=�6O   �� 6k   $�Remain Time :��6o   6�   �� 5M      -   5         ��Name$ 	ResidTime��5M       $��13(�63   $��75��6A   $��Right=�6L   ��6h   �� $ 
RemainTime�# Z $ TwoWay=�$ PropertyChanged=��� 5N      $��13(�6    $	��115��6.   $ Right=�6:   �� 6Q   $�SOC :��6U   6e   �� 5O      -   5        $��	chargeSoc��5O       $��13(�61   $��92��6?   ��6J   �� $	 SOC�# Z $ TwoWay=�$ PropertyChanged=�5P      �� 5Q      $��2��6   $ 
Horizontal=�6(   
�������6A   5S      5T      5U      5W      