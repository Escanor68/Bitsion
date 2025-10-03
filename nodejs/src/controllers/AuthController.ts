import { Request, Response } from 'express';
import jwt from 'jsonwebtoken';
import { LoginDto, LoginResponseDto } from '../dto/LoginDto';

export class AuthController {
  static async login(req: Request, res: Response): Promise<void> {
    try {
      const { username, password }: LoginDto = req.body;

      // Usuarios demo (en producción esto vendría de una base de datos)
      const users = [
        { username: 'admin', password: 'admin123', role: 'Admin' },
        { username: 'consultor', password: 'consultor123', role: 'Consultor' }
      ];

      const user = users.find(u => u.username === username && u.password === password);

      if (!user) {
        res.status(401).json({ message: 'Credenciales inválidas' });
        return;
      }

      const token = jwt.sign(
        { 
          username: user.username, 
          role: user.role 
        },
        process.env.JWT_SECRET!,
        { 
          expiresIn: process.env.JWT_EXPIRES_IN || '24h',
          issuer: process.env.JWT_ISSUER || 'PersonasABM',
          audience: process.env.JWT_AUDIENCE || 'PersonasABMUsers'
        }
      );

      const response: LoginResponseDto = {
        token,
        username: user.username,
        role: user.role,
        message: 'Login exitoso'
      };

      res.json(response);
    } catch (error) {
      console.error('Error en login:', error);
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }
}
