   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  ,    &QuickChargeDisplay.DeviceSettingWindow   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    0local clr-namespace:QuickChargeDisplay    <convert*clr-namespace:QuickChargeDisplay.Converter      � Title$  DeviceSettingWindow�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � SizeToContent$ WidthAndHeight=� � Topmost .  � 	Resources  �� 
TargetType   buttonStyle �� Property �� Value  textBlockStyle  textBoxStyle  
labelStyle��%A  &	        &	 �     &	 }    &	 n    �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 �   ���  �@��TextAlignment$ Right=���  �@��
IsReadOnly . ��  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �   ���  � ��
 �������  � |� .  .���� $
��1024��$	��768��.+��� $9��3image\12_Manual 동작화면\수동설정배경.gif���� $<��6image\10_관리자화면\아이콘\공장설정_ON.gif��	 �� Left$		 405��

 �� Top$
 23.338��$��46.676��$��50���� #��[ $��30(�$		 460��$
 28.75��$��36.25��$��201.25��장비 내부 설정�� -    ��Name$ closeWindow��$6��0image\12_Manual 동작화면\되돌아가기.gif��$
��Fill=�$	 945.189��$
 25.005��$��40��$��50����  K� ZIndex$ 1��#��[ $��30(�
���   �$	 119.988��$
 152.207��$��35.552��$��119.988��시간설정�� 
�������$��327.745��$��104.434��$	 111.1��$
 186.648����  �� Orientation$ 
Horizontal=�
�������$��35.552��$	 123.321��$
 197.758��$��305.525��.+��� #��[ .��날짜� .+��� #��[ $��3q�$	��100������  �� Path$
 Year� �� Mode$ TwoWay=�   QuickChargeDisplay.QCDV   
InnerSetup �� Source# Z  �� UpdateSourceTrigger$ PropertyChanged=��� -    ��Name$ rectYear��
������ $��3q��� #��[ .��/� .+��� #��[ $��3q�$��50������ $ Month�$ TwoWay=�# Z $ PropertyChanged=��� -   $ 	rectMonth��
������ $��3q��� #��[ .��/� .+��� #��[ $��3q�$��50������ $	 Day�$ TwoWay=�# Z $ PropertyChanged=��� -   $ rectDay��
������ $��3q��� $ 
Horizontal=�
�������$��35.552��$	 123.321��$
 243.309��$��248.864��.+��� #��[ .��시간� .+��� #��[ $��3q�$��50������ $
 Hour�$ TwoWay=�# Z $ PropertyChanged=��� -   $ rectHour��
������ $��3q��� #��[ .��:� .+��� #��[ $��3q�$��50������ $ Minute�$ TwoWay=�# Z $ PropertyChanged=��� -   $ 
rectMinute��
������ $��3q��� #��[ .��:� .+��� #��[ $��3q�$��50������ $ Seconds�$ TwoWay=�# Z $ PropertyChanged=��� -   $ rectSeconds��
������ $��3q�