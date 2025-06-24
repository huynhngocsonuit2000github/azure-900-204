# 1. Basic application for VM, .NET, deployment

- need to create the VM and config with those concepts
  🐧 Host .NET Web API on Azure VM (Ubuntu Linux)
  ✅ What You'll Do

1. Create a Linux VM on Azure (Ubuntu 22.04) [Done]
   - Create VM, and ensure port 22 (SSH) is open so we can connect
2. Install .NET SDK + runtime [Done]
   - Connect to VM and install .NET SDK to run and build ASP.NET Core Web API on linux, .NET runtime to allows the server to execute the app
3. Deploy your app using scp (or Git, or FTP) [Done]
   - Develop the app and build, publish the app. Then using SCP or SFTP to copy the publish to the Azure VM server
4. Use Nginx as a reverse proxy (for HTTP routing) [Done]
   - Basically the dotnet app running at port 5000 (not public). Nginx listens on port 80 and forward external request to the app
5. Configure firewall (NSG) to allow HTTP (port 80) [Done]
   - By default, Azure blocks HTTP (port 80). we have to open it to allow web traffic to reach the server and then Nginx then to .NET app
   - > API can be accesed via internet
6. Optionally add DNS + monitoring (optional) [Done]
   - Config the DNS for VM, in that case we can access the VM via domain-like address instead of IP
   - We can also track CPU usage, Memory, Disk, error
7. Connect Local host VS Code to VM [Done]
   - In VSCode install the extension Remote - SSH
   - Add ssh config: host, hostname, user
   - Then connect

# 2. New enhancement version

- 2.1. Use Docker to deploy instead
- 2.2. Enable HTTPS with Let's Encrypt (free SSL)
- 2.3. How to reproduce the CPU usage HIGH, Memory HIGH, Disk HIGH, error
- 2.4. Add CI/CD (GitHub to Azure VM)

## 2.1. Use Docker to deploy instead [Done]

The same way we perform configuration at step 1 just some different

- instead of build the code to Publish -> build code using Docker file to Docker images
- SCP to copy Publish to VM -> VM install Docker to pull docker images
- .NET runtime to run app -> use docker to run application and forward port 80 inside container to 80 to host machine

## 2.2. Enable HTTPS with Let's Encrypt (free SSL) [Done]

Using: Let’s Encrypt + NGINX + Certbot to enable HTTPS
Steps to do:

- Config DNS name for VM's public IP
- Port 80 and 443 opened in the Azure NSG (Network Security Group)
- Install Certbot and Nginx Plugin

  - Use Certbot to request certificate and auto-configure Nginx for my VM's custom domain
  - After that use the generated certificate to config for Nginx

- The flow when using
<!--
🌐 Browser/User
   |
   | HTTPS (port 443)
   v
🔒 Azure NSG (allows 443 + 80)
   |
   v
🖥️ Linux VM (public IP or DNS name)
   |
   | 443
   v
📦 NGINX (installed on VM, terminates HTTPS with Let's Encrypt cert)
   |
   | HTTP (port 5000 or 80)
   v
🐳 Docker Container (your .NET Web API app)

-->

## 2.3. How to reproduce the CPU usage HIGH, Memory HIGH, Disk HIGH, error [Done]

- Monitor with tools like **top, htop, iotop, or Azure Monitor**
- Write the APIs to increase RAM, CPU, Disk

## 2.4. Add CI/CD (Azure VM Jenkins to Azure VM) [Done]

- Install new VM
- Install docker
- Install Jenkins docker instance and reference to the docker.sock
