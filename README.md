# Car Rental Analysis Web Application

A data-driven web application designed to help multi-branch vehicle rental companies improve fleet utilisation and operational efficiency through machine learning and interactive visualisation.

## Overview

The Car Rental Analysis Dashboard combines machine learning with interactive visualisation to forecast demand trends and provide strategic recommendations for vehicle repositioning across branches throughout the year. This solution enables rental companies to transition from reactive to proactive fleet management, reducing idle time and increasing revenue by ensuring vehicles are available where and when they're needed most.

## Features

- **Demand Forecasting**: Machine learning models predict future rental demand based on historical data
- **Interactive Dashboard**: Real-time visualisation of fleet utilisation and performance metrics
- **Strategic Recommendations**: AI-powered suggestions for optimal vehicle allocation
- **Multi-branch Support**: Comprehensive management of vehicles across multiple locations
- **Seasonal Trend Analysis**: Identification of patterns and seasonal variations in rental demand
- **Operational Efficiency Metrics**: Key performance indicators for fleet management

## Technical Architecture

The solution consists of two main components:

1. **Backend Analysis Pipeline**
   - Machine learning engine for demand prediction
   - Historical data processing and pattern recognition
   - Seasonal trend analysis
   - Underutilisation risk assessment

2. **Web-based Dashboard**
   - Built with Streamlit for interactive visualisations
   - Embedded within ASP.NET Core web application
   - Real-time data updates and insights
   - User-friendly interface for management decisions

## Prerequisites

- Python 3.8 or higher
- .NET Core 6.0 or higher
- SQL Server (or compatible database)
- Required Python packages (see requirements.txt)
- Required .NET packages (see .csproj file)

## Installation

1. Clone the repository:
   ```bash
   git clone [repository-url]
   cd CarRentalWebApp
   ```

2. Install Python dependencies:
   ```bash
   pip install -r requirements.txt
   ```

3. Install .NET dependencies:
   ```bash
   dotnet restore
   ```

4. Configure the database connection in `appsettings.json`

5. Run database migrations:
   ```bash
   dotnet ef database update
   ```

## Running the Application

1. Start the backend services:
   ```bash
   dotnet run
   ```

2. Launch the Streamlit dashboard:
   ```bash
   streamlit run dashboard/app.py
   ```

3. Access the application at `http://localhost:5000`

## Usage

1. Log in to the dashboard using your credentials
2. Navigate through different sections:
   - Fleet Overview
   - Demand Forecasts
   - Branch Performance
   - Vehicle Allocation
   - Seasonal Analysis
3. Use the interactive filters to customise your view
4. Export reports and recommendations as needed

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

