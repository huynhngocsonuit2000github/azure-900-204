# 1. Basic application

- [WIP]: step-by-step guide to create a Site-to-Site VPN connection in Azure, using the Azure Portal (UI) and Azure CLI.

1. Create a **Virtual Network** [Done]
   - Create the VNet within creating the Subnet
2. Create the **VPN Gateway**
  - Create a **Public IP Address** for the **VPN Gateway**
3. Create **Local Network Gateway** (Represents On-Prem)
  - curl ifconfig.me to get the public IP Address
  - ifconfig to get Private IP/Subnet
4. Create the **VPN Connection**
  - Createe the connection **Site-to-Site** between **Azure Network with On-prem** we just created
  - Assign the key, which is used to config the same in VPN device
  - use ifconfig en0 to check the IPs Assigned to your mac
    <!-- inet 192.168.1.42 -->
    <!-- inet 192.168.99.100 -->

5. Configure Your On-Prem VPN Device
6. Verify the VPN Connection

<!-- Why do we use VPN Gateway? -->
<!-- What is the benifit? -->
<!-- Some of real cases -->

# 2. Enhancement