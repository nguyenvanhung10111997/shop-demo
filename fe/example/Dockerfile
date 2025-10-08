# -----------------------
# 1. Build Angular App
# -----------------------
FROM node:lts AS build

# Set working directory
WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm ci

# Copy source code
COPY . .

# Build Angular app in production mode
RUN npm run build -- --configuration production

# -----------------------
# 2. Serve with Nginx
# -----------------------
FROM nginx:latest

# Remove default nginx static files
RUN rm -rf /usr/share/nginx/html/*

# Copy Angular build from build stage
COPY --from=build /app/dist/* /usr/share/nginx/html/

# Copy custom nginx configuration (optional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Run Nginx
CMD ["nginx", "-g", "daemon off;"]
