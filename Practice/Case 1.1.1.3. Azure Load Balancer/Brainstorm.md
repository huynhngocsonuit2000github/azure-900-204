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

- 2.1. Add SSL (HTTPS)
- 2.2. Test with multiple VMs
- 2.3. Switch to Internal Load Balancer
- 2.4. Add autoscaling for VM pool
