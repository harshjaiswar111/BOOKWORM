package com.example.aop;

import org.aspectj.lang.JoinPoint;
import org.aspectj.lang.annotation.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

@Aspect
@Component
public class AuthLoggingAspect {

    private static final Logger log =
            LoggerFactory.getLogger(AuthLoggingAspect.class);

    /**
     * 🎯 Pointcut:
     * All methods inside AuthService
     * (login, register, oauth helpers etc.)
     */
    @Pointcut("execution(* com.example.Services.AuthService.*(..))")
    public void authServiceMethods() {}

    /**
     * ✅ BEFORE method execution
     */
    @Before("authServiceMethods()")
    public void logBefore(JoinPoint joinPoint) {
        log.info(
            "➡️ AUTH START : Method = {}",
            joinPoint.getSignature().getName()
        );
    }

    /**
     * ✅ AFTER successful execution
     */
    @AfterReturning("authServiceMethods()")
    public void logAfterSuccess(JoinPoint joinPoint) {
        log.info(
            "✅ AUTH SUCCESS : Method = {}",
            joinPoint.getSignature().getName()
        );
    }

    /**
     * ❌ IF exception occurs
     */
    @AfterThrowing(
        pointcut = "authServiceMethods()",
        throwing = "ex"
    )
    public void logAfterFailure(JoinPoint joinPoint, Exception ex) {
        log.error(
            "❌ AUTH FAILED : Method = {} | Reason = {}",
            joinPoint.getSignature().getName(),
            ex.getMessage()
        );
    }
}
