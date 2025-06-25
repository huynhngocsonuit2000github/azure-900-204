# 1. Basic application

- [WIP]: Expose your Web API via a Public IP + Load Balancer, with the ability to scale to more VMs.

Architecture

<!--

Client (Internet)
       ↓
[Public IP] (Load Balancer)
       ↓
  Backend Pool (VM1, VM2...)
       ↓
   Web API on Port 80

 -->

1. Create simple 2 VMs, running Nginx Webapp [Done]
2. Create Public Load Balancer instance [Done]
   - The same region VM placed
3. Create Backend Pool [Done]
   - To associate with the VNet and Region
4. Create Health Probe
   - Load Balancer uses it to check check whether forwad the request to healthy VM or not
5. Add Load Balancing Rule
   - Route the incoming request and outcoming response
6. Test It
<!--
       C:\Users\hnson>curl http://135.237.7.95/
       <h1>Hello Nginx from VM2</h1>

       C:\Users\hnson>curl http://135.237.7.95/
       hello nginx from VM 1

       C:\Users\hnson>curl http://135.237.7.95/
       <h1>Hello Nginx from VM2</h1>

       C:\Users\hnson>curl http://135.237.7.95/
       hello nginx from VM 1

       C:\Users\hnson>curl http://135.237.7.95/
       <h1>Hello Nginx from VM2</h1>
-->

# 2. Enhance

- 2.1. Add DNS and SSL (HTTPS)
- 2.2. Switch to Internal Load Balancer
- 2.3. Add autoscaling for VM pool

## 2.1. Add DNS and SSL (HTTPS)

- Chose the correct **Public IP Address** that associate with the Load Balancer
- Config for this IP, add the DNS
- Basically, Load Balancer works at the Layer 4, so it does not support SSL termination. But we can work around by
  - Install SSL in all of the VMs, and expose the port to 443
  - Config LB rule to forward port 443
    -> But this way is not recommended
