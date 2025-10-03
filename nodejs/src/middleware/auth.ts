import { Request, Response, NextFunction } from 'express';
import jwt from 'jsonwebtoken';

export interface AuthRequest extends Request {
  user?: {
    username: string;
    role: string;
  };
}

export const authenticateToken = (req: AuthRequest, res: Response, next: NextFunction) => {
  const authHeader = req.headers['authorization'];
  const token = authHeader && authHeader.split(' ')[1];

  if (!token) {
    return res.status(401).json({ message: 'Token de acceso requerido' });
  }

  jwt.verify(token, process.env.JWT_SECRET!, (err, decoded: any) => {
    if (err) {
      return res.status(403).json({ message: 'Token inválido' });
    }

    req.user = {
      username: decoded.username,
      role: decoded.role
    };
    next();
  });
};

export const requireRole = (roles: string[]) => {
  return (req: AuthRequest, res: Response, next: NextFunction) => {
    if (!req.user) {
      return res.status(401).json({ message: 'No autenticado' });
    }

    if (!roles.includes(req.user.role)) {
      return res.status(403).json({ message: 'No tiene permisos para realizar esta acción' });
    }

    next();
  };
};
