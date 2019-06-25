<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="APIS2.Azure" generation="1" functional="0" release="0" Id="15eeb1e7-ee8b-4377-96e9-86622eb50576" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="APIS2.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="APIS2:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/APIS2.Azure/APIS2.AzureGroup/LB:APIS2:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="APIS2:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/APIS2.Azure/APIS2.AzureGroup/MapAPIS2:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="APIS2Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/APIS2.Azure/APIS2.AzureGroup/MapAPIS2Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:APIS2:Endpoint1">
          <toPorts>
            <inPortMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAPIS2:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAPIS2Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="APIS2" generation="1" functional="0" release="0" software="F:\MY_Programs_and_Applications\CSharp\APIS2\APIS2.Azure\csx\Debug\roles\APIS2" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;APIS2&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;APIS2&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2Instances" />
            <sCSPolicyUpdateDomainMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="APIS2UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="APIS2FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="APIS2Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="6640201f-af67-45fa-98bf-37c67c56ceba" ref="Microsoft.RedDog.Contract\ServiceContract\APIS2.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="40e419cd-686c-450e-a16d-f3bfb0efdbc1" ref="Microsoft.RedDog.Contract\Interface\APIS2:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/APIS2.Azure/APIS2.AzureGroup/APIS2:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>