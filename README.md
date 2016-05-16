# clever_awusb
A tool to remotely manage AnywhereUSB via web-interface. Includes a simple scripting mechanism for connecting and disconnecting ports.

# тестирование USB-устройств

Для проверки того, что устройство успешно подключено, используется вызов <Connect device="{deviceId}">, где {deviceId} - уникальная строка или часть строки, уникально идентифицирующая устройство.

Получить список устройств, и выбрать из него часть строки идентификатора можно с помощью команды --list:
cvawusb_batch.exe --list

```
AWUSB\AWUSBRHUB_0002\CONCENTRATOR_172.16.31.50
AWUSB\AWUSBHC_0002\HOSTCONTROLLER_172.16.31.50_0
USB\ROOT_HUB\5&3BB57B&0
USB\ROOT_HUB20\5&6106580&0
AWUSB\ROOT_HUB_0002\ROOT_HUB_172.16.31.50_0
USB\VID_0E0F&PID_0002\6&B77DA92&0&2
<b>AWUSB\VID_0A89&PID_0030\0A89&0030&AC101F32&1&A</b>
USB\VID_0E0F&PID_0003\6&B77DA92&0&1
USB\VID_0E0F&PID_0003&MI_00\7&2A7D3009&0&0000
USB\VID_0E0F&PID_0003&MI_01\7&2A7D3009&0&0001
```
Интересующим нас устройством в приведенном списке является:
```
AWUSB\VID_0A89&PID_0030\<b>0A89&0030&AC101F32&1&A</b>
```

Определить идентификатор нужного устройства можно подключая устройство, запрашивая список, отключая устройство, запрашивая список и сравнивая вывод.

В качестве уникального идентификатора устройства для конфигурационного файла можно взять последний сегмент.

Обращаем внимание, что в XML формате символ амперсанда следует заменять на &amp;


Пример конфигурации с проверками:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Flow>
    <Item id="check_devices">
      <Sequence>
		<Call id="connect_10"/>
        <Connect device="0A89&amp;0030&amp;AC101F32&amp;1&amp;AB"></Connect>
		<Call id="connect_12"/>
        <Connect device="23A0&amp;0002&amp;AC101F32&amp;1&amp;C"></Connect>
      </Sequence>
    </Item>
  <Item id="disconnect_all">
    <Sequence>
      <SetPort ip="172.16.31.50" port="10" value="0"></SetPort>
      <SetPort ip="172.16.31.50" port="12" value="0"></SetPort>
    </Sequence>
  </Item>
   <Item id="connect_10" title="Connect port 10">
    <Sequence>
	  <Call id="disconnect_all"/>
      <SetPort ip="172.16.31.50" port="10" value="10"></SetPort>
    </Sequence>
  </Item>
   <Item id="connect_12" title="Connect port 12">
    <Sequence>
	  <Call id="disconnect_all"/>
      <SetPort ip="172.16.31.50" port="12" value="10"></SetPort>
    </Sequence>
  </Item>
</Flow>
```
