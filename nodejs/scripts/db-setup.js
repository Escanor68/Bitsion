#!/usr/bin/env node

/**
 * Script para configurar y verificar la base de datos SQLite
 */

const { execSync } = require('child_process');
const fs = require('fs');
const path = require('path');

console.log('🔧 Configurando base de datos SQLite...\n');

// Verificar si el archivo de base de datos existe
const dbPath = path.join(__dirname, '..', 'PersonasABM.db');
const dbExists = fs.existsSync(dbPath);

if (dbExists) {
  console.log('✅ Base de datos SQLite encontrada:', dbPath);
  const stats = fs.statSync(dbPath);
  console.log(`📊 Tamaño: ${(stats.size / 1024).toFixed(2)} KB`);
  console.log(`📅 Última modificación: ${stats.mtime.toLocaleString()}`);
} else {
  console.log(
    '⚠️  Base de datos SQLite no encontrada. Se creará al iniciar el servidor.'
  );
}

// Verificar dependencias
console.log('\n🔍 Verificando dependencias...');
try {
  const sqlite3Version = execSync('npm list sqlite3', { encoding: 'utf8' });
  console.log('✅ SQLite3 instalado correctamente');
} catch (error) {
  console.log('❌ SQLite3 no está instalado. Ejecutando: npm install sqlite3');
  execSync('npm install sqlite3', { stdio: 'inherit' });
}

console.log('\n🚀 Para iniciar el servidor con SQLite:');
console.log('   npm run dev    # Modo desarrollo');
console.log('   npm start      # Modo producción');
console.log('\n📁 Ubicación de la base de datos: ./PersonasABM.db');
console.log('🔧 Para verificar la base de datos: sqlite3 PersonasABM.db');
