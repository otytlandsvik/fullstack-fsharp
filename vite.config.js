import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";
// import fable from "vite-plugin-fable";
import { dependencies } from './package.json';
import { resolve } from 'path';

const vendorDeps = ['react', 'react-dom']

const chunksFromDeps = (deps, vendorDeps) => {
  const chunks = {}
  Object.keys(deps).forEach((key) => {
    if (vendorDeps.includes(key) || key.startsWith('@fluentui')) {
      return
    }
    chunks[key] = [key]
  })
  return chunks
}

const serverPort = 8085
const clientPort = 8083

const proxy = {
  target: `http://0.0.0.0:${serverPort}/`,
  changeOrigin: false,
  secure: false,
  ws: true
}

/** @type {import('vite').UserConfig} */
export default defineConfig({
  plugins: [react()],
  // assetsInclude: ['**/*.fs'],
  root: ".",
  publicDir: resolve(__dirname, "./public"),
  build: {
    outDir: resolve(__dirname, "./dist/public"),
    emptyOutDir: true,
    sourcemap: false,
    rollupOptions: {
      input: {
        main: resolve(__dirname, "./src/Client/index.html")
      },
      output: {
        manualChunks: {
          vendor: vendorDeps,
          ...chunksFromDeps(dependencies, vendorDeps)
        },
        entryFileNames: 'js/[name][hash].js',
        chunkFileNames: 'js/[name][hash].chunk.js',
        assetFileNames: '[ext]/[name][hash].[ext]'
      },
    }
  },
  server: {
    host: '0.0.0.0',
    port: clientPort,
    strictPort: true,
    proxy: {
        '/api': proxy,
    },
    watch: {
        ignored: [
            "bin",
            "obj",
            "**/*.fs"
        ],
    }
  },
  preview: {
    host: '0.0.0.0',
    port: clientPort,
    strictPort: true,
    proxy: {
        '/api': proxy,
    }
  }
});
