## 一些说明

> 基于ASP.Net Core 2.1和OAuth2的RESTful风格的WebAPI系统

> .Net Core学习资料：http://note.youdao.com/noteshare?id=dfe9debb2e7f4f3fe6de05a2a0493fb0

> Linux进程管理：http://note.youdao.com/noteshare?id=f80cd5bd2898bfda522f359aab3286b0

> API文档（本地部署）：http://localhost:5001/swagger/

> ~~发布后将 MyWebAPI\Host\FFmpeg 文件夹拷贝到发布目录~~

> 转换失败请检查ffmpeg文件是否有权限

> 系统架构

 ![进入应用](./Doc/WebAPI架构图.png)

> 一些开发原则：

1、依赖注入由构造函数改为按需实例化

2、实例化后可重复使用，无需提炼变量

3、所有校验如无必要改为抛出业务类异常（正常响应200，ValidateException用于业务校验，BusinessException一般用于前端提示或特殊处理）

4、ArgumentException：BadRequest；BusinessException：OK（IsSuccess=false）；其余异常：InternalServerError

5、所有方法如无必要直接返回结果无需二次封装，比如controller方法无需返回JsonResult

6、请求响应数据格式：
 * 异常响应，400或500这类，建议api调用端直接记录无视响应格式
 * 200响应：
 ```javascript
{
    "isSuccess": true/false,
    "error": {  // isSuccess=false
        "code": "xxx",
        "message": "xxx"
    },
    "data": {}  // isSuccess=true
}
```

7、如无必要不主动捕获异常

8、~~手动记录日志请用LysLog.BizLogger~~


> 参考资料
 * [IdentityServer4官方样例](https://github.com/IdentityServer/IdentityServer4.Samples/tree/release/Quickstarts)
 * [.Net Core2的IdentityServer4使用教程](http://www.cnblogs.com/stulzq/p/7493745.html)
 * [拷贝到Linux上运行提示无效文件](https://www.iyunv.com/thread-408970-1-1.html)
