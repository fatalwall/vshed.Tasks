# vshed.Tasks
Configurable task runner for exteranl calls


## Example - Enumerating Task Items

### Configuraiton File
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="Tasks" type="vshed.Tasks.Tasks, vshed.Tasks" requirePermission="false"/>
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
  </startup>
  <Tasks>
    <Task Type="Process" 
          Name="ipconfig Fail" 
          FileName="ipconfig" 
          Arguments="/all" 
          SuccessRegex="\s{3}Connection-specific DNS Suffix[\s,\.]*: \R\s{3}Description[\s,\.]*: PANTECH UML290 Mobile Broadband.*\R\s{3}(Physical Address)[\s,\.]*: (..-..-..-..-..-..)\R\s{3}DHCP Enabled[\s,\.]*: (:?No|Yes)\R\s{3}Autoconfiguration Enabled[\s,\.]*: Yes\R\s{3}Link-local IPv6 Address[\s,\.]*: .{1,4}::.{1,4}:.{1,4}:.{1,4}:.......\(Preferred\) \R\s{3}(IPv4 Address)[\s,\.]*: (\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\(Preferred\) \R\s{3}(Subnet Mask)[\s,\.]*: (\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\R\s{3}(Default Gateway)[\s,\.]*: (\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})" 
          />
    <Task Type="Process" 
          Name="ipconfig Success" 
          FileName="ipconfig" 
          Arguments="/all" 
          SuccessRegex="(IPv4 Address)[\s,\.]*: (\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})" 
          />
    <Task Type="Process" Name="Pause" FileName="timeout" Arguments="/t 60" />
    <Task Type="Process" Name="Pause2" FileName="ping " Arguments="localhost -n 60" />
    <Task Type="SerialCommand" 
          Name="Get Cellular Modem Number" 
          Command="AT+CNUM" 
          SuccessRegex="OK" PortName="COM 3"
          BaudRate="9600" 
          DataBits="8" 
          Parity="None" 
          StopBits="One" 
          Encoding="ISO-8859-1" 
          DtrEnable="true" 
          RtsEnable="true" 
          />
    <Task Type="SerialCommand" 
          Name="Get Cellular Modem IMEI" 
          Command="AT+CGSN" SuccessRegex="OK" 
          PortName="COM 3" 
          BaudRate="9600" 
          DataBits="8" Parity="None" 
          StopBits="One" 
          Encoding="ISO-8859-1" 
          DtrEnable="true" 
          RtsEnable="true" 
          />
    <Task Type="SerialCommand" 
          Name="Get Cellular Modem ICCID" 
          Command="AT+ICCID" SuccessRegex="OK" 
          PortName="COM 3" 
          BaudRate="9600" DataBits="8" 
          Parity="None" 
          StopBits="One" Encoding="ISO-8859-1" 
          DtrEnable="true" 
          RtsEnable="true" />
    <Processes>
      <Process Name="Pause3" FileName="ping " Arguments="localhost -n 60" />
      <Process Name="Pause2" FileName="ping " Arguments="localhost -n 60" />
    </Processes>
    <SerialCommands>
      <SerialCommand Name="Get Cellular Modem ICCID3" 
                     Command="AT+ICCID" SuccessRegex="OK" 
                     PortName="COM 3" 
                     BaudRate="9600" DataBits="8" 
                     Parity="None" StopBits="One" 
                     Encoding="ISO-8859-1" 
                     DtrEnable="true" 
                     RtsEnable="true" />
    </SerialCommands>
  </Tasks>
</configuration>
```
### Code
```C#
vshed.Tasks.Tasks Settings = ConfigurationManager.GetSection("Tasks") as vshed.Tasks.Tasks;
Settings.Processes["Confirming Network is Up"].Start();
System.Diagnostics.Debug.WriteLine(String.Format("Processes: {0}", Settings.Processes.Count));
foreach (var p in Settings.Processes) 
  System.Diagnostics.Debug.WriteLine(String.Format("  {0}", p.ToString()));
System.Diagnostics.Debug.WriteLine(String.Format("SerialCommands: {0}", Settings.SerialCommands.Count));
foreach (var p in Settings.SerialCommands) 
  System.Diagnostics.Debug.WriteLine(String.Format("  {0}", p.ToString()));
```

### Debug Output
```
Processes: 5
  Pause3
  Pause2
  ipconfig Fail
  ipconfig Success
  Pause
SerialCommands: 4
  Get Cellular Modem ICCID3
  Get Cellular Modem Number
  Get Cellular Modem IMEI
  Get Cellular Modem ICCID
```
