import { Injectable, Logger, NestMiddleware } from '@nestjs/common';
import { Request, Response, NextFunction } from 'express';

@Injectable()
export class RequestLoggerMiddleware implements NestMiddleware {
  private logger = new Logger('HTTP');

  use(request: Request, response: Response, next: NextFunction): void {
    const { ip, method, path: url } = request;
    const userAgent = request.get('user-agent') ?? '';

    response.on('close', () => {
      const { statusCode } = response;

      const body = JSON.stringify(request.body);
      const query = JSON.stringify(request.query);

      const content = body.length > 2 ? body : query.length > 2 ? query : '';
      const params = JSON.stringify(request.params);

      // this.logger.log(
      //   `${method} ${url} ${statusCode} | params: ${params} | content: ${content} | useragent: ${userAgent} | IP: ${ip}`,
      // );
    });

    next();
  }
}
