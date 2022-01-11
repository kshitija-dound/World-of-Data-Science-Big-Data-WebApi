# BankMarketing
Bank Marketing dataset is obtained from UCI machine learning repository [Source: [Moro et al., 2014] S. Moro, P. Cortez and P. Rita. A Data-Driven Approach to Predict the Success of Bank Telemarketing. Decision Support Systems, Elsevier, 62:22-31, June 2014 ] and has information about customers on portugal bank marketing campaign who have subsribed to the term deposit.
We have performed exploratory data analysis on dataset and created new features using bucketizing on age, min max scaling on numerical features.
The dataset was imbalanced with large no and few yes values on output/y column. We balanced the dataset adding weights to each row of the dataset.
We have also ran Gradient boosting algorithm,Random Forest,Logistic Regression and SVC models on the dataset and passed weightcol =weight for balancing the dataset.

Business Conclusion:
We found out that numerical features like consumerpriceindex, employeevarrate, pdays, housing_index, default_index, previous, contact_index, euribor3m, nremployed, campaign have 
high correlation with clients subscribing to term deposit.
